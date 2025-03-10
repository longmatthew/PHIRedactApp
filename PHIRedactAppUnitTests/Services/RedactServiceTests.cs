using PHIRedactApp.Services;

namespace PHIRedactAppUnitTests;

public class RedactServiceTests
{
    private readonly RedactService _redactService;

    public RedactServiceTests()
    {
        _redactService = new RedactService();
    }

    [Fact]
    public void RedactPhiAsync_ShouldReturnEmptyString_WhenInputIsEmpty()
    {
        // Act
        var result = _redactService.RedactPhiAsync("");

        // Assert
        Assert.Equal("", result);
    }

    [Fact]
    public void RedactPhiAsync_ShouldReturnNull_WhenInputIsNull()
    {
        // Act
        var result = _redactService.RedactPhiAsync(null);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void RedactPhiAsync_ShouldRedactPatientName()
    {
        // Arrange
        string input = "Patient Name: John Doe";
        string expected = "Patient Name: [REDACTED]";

        // Act
        var result = _redactService.RedactPhiAsync(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void RedactPhiAsync_ShouldRedactDate()
    {
        // Arrange
        string input = "Appointment Date: 05/14/2024";
        string expected = "Appointment Date: [REDACTED]";

        // Act
        var result = _redactService.RedactPhiAsync(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void RedactPhiAsync_ShouldRedactSSN()
    {
        // Arrange
        string input = "SSN: 123-45-6789";
        string expected = "SSN: [REDACTED]";

        // Act
        var result = _redactService.RedactPhiAsync(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void RedactPhiAsync_ShouldRedactAddress()
    {
        // Arrange
        string input = "Address: 1234 Main St, Springfield, IL";
        string expected = "Address: [REDACTED]";

        // Act
        var result = _redactService.RedactPhiAsync(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void RedactPhiAsync_ShouldRedactPhoneNumber()
    {
        // Arrange
        string input = "Phone: (555) 123-4567";
        string expected = "Phone: [REDACTED]";

        // Act
        var result = _redactService.RedactPhiAsync(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void RedactPhiAsync_ShouldRedactEmail()
    {
        // Arrange
        string input = "Email: john.doe@example.com";
        string expected = "Email: [REDACTED]";

        // Act
        var result = _redactService.RedactPhiAsync(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void RedactPhiAsync_ShouldRedactMedicalRecordNumber()
    {
        // Arrange
        string input = "MRN-12345678";
        string expected = "[REDACTED]";

        // Act
        var result = _redactService.RedactPhiAsync(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void RedactPhiAsync_ShouldNotModifyNonPHIData()
    {
        // Arrange
        string input = "This is a regular sentence without PHI.";
        string expected = "This is a regular sentence without PHI.";

        // Act
        var result = _redactService.RedactPhiAsync(input);

        // Assert
        Assert.Equal(expected, result);
    }
}
