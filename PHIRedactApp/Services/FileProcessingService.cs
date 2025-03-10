using Microsoft.Extensions.Logging;

namespace PHIRedactApp.Services;

public interface IFileProcessingService
{
    public bool ProcessRedactedFile(string redactedLabOrderText, string fileName);
}

public class FileProcessingService(ILogger<FileProcessingService> logger) : IFileProcessingService
{
    private readonly ILogger<FileProcessingService> _logger = logger;

    public bool ProcessRedactedFile(string redactedLabOrderText, string fileName)
    {
        var tempPath = Path.Combine(Path.GetTempPath(), "sanitized");
        string redactedFileName = Path.GetFileNameWithoutExtension(fileName) + "_sanitized.txt";
        string redactedFilePath = Path.Combine(tempPath, redactedFileName);

        if (!Directory.Exists(tempPath))
        {
            Directory.CreateDirectory(tempPath);
        }

        try
        {
            int counter = 1;
            while (File.Exists(redactedFilePath))
            {
                redactedFilePath = Path.Combine(tempPath, $"{fileName}_{counter}_sanitized.txt");
                counter++;
            }

            File.WriteAllText(redactedFilePath, redactedLabOrderText);
            _logger.LogInformation($"Sanitized file saved at: {redactedFilePath}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"File write unsuccessful. Message: {ex.Message}");
            return false;
        }
    }
}
