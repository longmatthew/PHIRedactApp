using MediatR;
using PHIRedactApp.Commands;
using PHIRedactApp.Services;

namespace PHIRedactApp.Handlers;

public class RedactLabOrderHandler(IRedactService redactionService, IFileProcessingService fileProcessingService) : IRequestHandler<RedactLabOrderCommand, bool>
{
    private readonly IRedactService _redactionService = redactionService;
    private readonly IFileProcessingService _fileProcessingService = fileProcessingService;

    public async Task<bool> Handle(RedactLabOrderCommand request, CancellationToken cancellationToken)
    {
        var redactedContent = _redactionService.RedactPhiAsync(request.LabOrderText);
        return await Task.FromResult(_fileProcessingService.ProcessRedactedFile(redactedContent, request.FileName));
    }
}
