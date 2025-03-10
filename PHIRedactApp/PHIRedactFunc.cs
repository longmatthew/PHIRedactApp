using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using PHIRedactApp.Commands;
using System.Net;

namespace PHIRedactApp;

public class PHIRedactFunc(IMediator mediator, ILogger<PHIRedactFunc> logger)
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<PHIRedactFunc> _logger = logger;

    [Function(nameof(PHIRedactApp))]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req, string fileName, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return await CreateResponse(req, HttpStatusCode.BadRequest, "File name is empty.");
        }

        _logger.LogInformation("Processing uploaded lab order text file.");

        using var reader = new StreamReader(req.Body);
        string fileContent = await reader.ReadToEndAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(fileContent))
        {
            return await CreateResponse(req, HttpStatusCode.BadRequest, "Uploaded file is empty.");
        }

        bool successfulUpload = false;
        try
        {
            successfulUpload = await _mediator.Send(new RedactLabOrderCommand(fileContent, fileName), cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing lab order: {ex.Message}");
            return await CreateResponse(req, HttpStatusCode.InternalServerError, "Encountered Error. Invalid file format.");
        }

        if (successfulUpload)
        {
            return await CreateResponse(req, HttpStatusCode.OK, "File upload successful.");
        }
        else
        {
            return await CreateResponse(req, HttpStatusCode.BadRequest, "File upload unsuccessful.");
        }
    }

    private static async Task<HttpResponseData> CreateResponse(HttpRequestData req, HttpStatusCode statusCode, string message)
    {
        var response = req.CreateResponse(statusCode);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        await response.WriteStringAsync(message);
        return response;
    }
}
