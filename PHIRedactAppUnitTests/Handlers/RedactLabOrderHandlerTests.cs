using Moq;
using PHIRedactApp.Commands;
using PHIRedactApp.Handlers;
using PHIRedactApp.Services;

namespace PHIRedactAppUnitTests.Handlers;

public class RedactLabOrderHandlerTests
{
    private readonly Mock<IRedactService> _mockRedactService = new Mock<IRedactService>();
    private readonly Mock<IFileProcessingService> _mockFileProcessingService = new Mock<IFileProcessingService>();
    private readonly RedactLabOrderHandler _handlerInTest;

    public RedactLabOrderHandlerTests()
    {
        _handlerInTest = new RedactLabOrderHandler(_mockRedactService.Object, _mockFileProcessingService.Object);
    }

    [Fact]
    public void RedactLabOrderHandler_SuccessfulServices_ReturnsTrue()
    {
        // Arrange
        string redactedFileText = "Redacted Lab Order Text";
        _mockRedactService.Setup(x => x.RedactPhiAsync(It.IsAny<string>())).Returns(redactedFileText);
        _mockFileProcessingService.Setup(x => x.ProcessRedactedFile(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
        RedactLabOrderCommand redactLabOrderCommand = new RedactLabOrderCommand("Valid Lab Order Text", "sample.txt");

        // Act
        var response = _handlerInTest.Handle(redactLabOrderCommand, new CancellationToken());

        // Assert
        Assert.True(response.Result);
    }
}
