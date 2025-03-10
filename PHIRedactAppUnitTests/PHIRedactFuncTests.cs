using System.Collections.Specialized;
using System.Net;
using System.Text;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;
using PHIRedactApp;
using PHIRedactApp.Commands;

namespace PHIRedactAppUnitTests
{
    public class PHIRedactFuncTests
    {
        private readonly Mock<IMediator> _mockMediatr = new Mock<IMediator>();
        private readonly Mock<ILogger<PHIRedactFunc>> _mockLogger = new Mock<ILogger<PHIRedactFunc>>();
        private readonly PHIRedactFunc _functionInTest;

        public PHIRedactFuncTests()
        {
            _functionInTest = new PHIRedactFunc(_mockMediatr.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task PHIRedactFunc_NoFileName_ReturnsBadRequest()
        {
            // Arrange
            var request = HttpRequestDataSetup("", "Sample file content");

            // Act
            var response = await _functionInTest.Run(request, "", CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PHIRedactFunc_EmptyFileContent_ReturnsBadRequest()
        {
            // Arrange
            var request = HttpRequestDataSetup("sample.txt", "");

            // Act
            var response = await _functionInTest.Run(request, "sample.txt", CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PHIRedactFunc_ValidFile_ReturnsOk()
        {
            // Arrange
            var request = HttpRequestDataSetup("sample.txt", "Valid file content");

            _mockMediatr
                .Setup(x => x.Send(It.IsAny<RedactLabOrderCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var response = await _functionInTest.Run(request, "sample.txt", CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PHIRedactFunc_InvalidFile_ReturnsBadRequest()
        {
            // Arrange
            var request = HttpRequestDataSetup("sample.txt", "Valid file content");

            _mockMediatr
                .Setup(x => x.Send(It.IsAny<RedactLabOrderCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var response = await _functionInTest.Run(request, "sample.txt", CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PHIRedactFunc_InternalError_ReturnsServerError()
        {
            // Arrange
            var request = HttpRequestDataSetup("sample.txt", "Some file content");

            _mockMediatr
                .Setup(x => x.Send(It.IsAny<RedactLabOrderCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Simulated processing error"));

            // Act
            var response = await _functionInTest.Run(request, "sample.txt", CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        private HttpRequestData HttpRequestDataSetup(string fileName, string body, Dictionary<string, StringValues>? query = null)
        {
            var queryItems = new NameValueCollection();

            if (query != null)
            {
                foreach (var item in query)
                {
                    queryItems.Add(item.Key, item.Value);
                }
            }

            queryItems.Add("fileName", fileName);

            var context = new Mock<FunctionContext>();
            var request = new Mock<HttpRequestData>(context.Object);
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(body));

            request.Setup(x => x.Body).Returns(memoryStream);
            request.Setup(x => x.Query).Returns(queryItems);
            request.Setup(r => r.CreateResponse()).Returns(() =>
            {
                var response = new Mock<HttpResponseData>(context.Object);
                response.SetupProperty(r => r.Headers, new HttpHeadersCollection());
                response.SetupProperty(r => r.StatusCode);
                response.SetupProperty(r => r.Body, new MemoryStream());
                return response.Object;
            });

            return request.Object;
        }
    }
}
