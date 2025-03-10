using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace PHIRedactWebApp.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private const string FunctionUrl = "http://localhost:7215/api/PHIRedactApp"; // Update with your function URL

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> UploadFilesAsync(string fileName, Stream fileStream)
    {
        using var content = new MultipartFormDataContent();
        var fileContent = new StreamContent(fileStream);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("text/plain");

        content.Add(fileContent, "file", fileName);
        var requestUri = $"{FunctionUrl}?fileName={Uri.EscapeDataString(fileName)}";

        HttpResponseMessage? response = null;
        try
        {
            response = await _httpClient.PostAsync(requestUri, content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            Console.WriteLine($"Error: {await response.Content.ReadAsStringAsync()}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            return false;
        }
    }
}
