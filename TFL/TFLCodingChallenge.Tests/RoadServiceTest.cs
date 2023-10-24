using Moq.Protected;
using Moq;
using System.Net;
using TFLCodingChallenge.Service;

namespace TFLCodingChallenge.Tests
{
    [TestClass]
    public class RoadServiceTest
    {
        [TestMethod]
        public async Task GetRoadStatusReturnsRoadDetailsWhenValidRoadIdIsProvided()
        {
            string roadName = "A2";
            string expectedStatusSeverity = "Good";
            string expectedStatusSeverityDescription = "No Exceptional Delays";

            // Arrange
            var jsonResponse = "[{\"$type\":\"Tfl.Api.Presentation.Entities.RoadCorridor, Tfl.Api.Presentation.Entities\"," +
                "\"id\":\"a2\"," +
                "\"displayName\":\"A2\"," +
                "\"statusSeverity\":\"Good\"," +
                "\"statusSeverityDescription\":\"No Exceptional Delays\"," +
                "\"url\":\"/Road/a2\"}]";

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var httpClientMock = new Mock<HttpClient>();
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse, System.Text.Encoding.UTF8, "application/json"),
                }).Verifiable();

            //used real http client with mocked handler here

            var httpClient = new HttpClient(httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri($"https://api.tfl.gov.uk/{roadName}/?app_id=x&aapp_key=y"),
            };

            httpClientFactoryMock.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var roadService = new RoadService(httpClientFactoryMock.Object);

            // Act
            var responseDTo = await roadService.GetRoadStatus($"https://api.tfl.gov.uk/{roadName}/?app_id=x&aapp_key=y");

            var roadDetails = responseDTo.RoadDetails;

            // Assert
            Assert.AreEqual(roadDetails.DisplayName, roadName);
            Assert.AreEqual(roadDetails.StatusSeverity, expectedStatusSeverity);
            Assert.AreEqual(roadDetails.StatusSeverityDescription, expectedStatusSeverityDescription);
        }

        [TestMethod]
        public async Task GetRoadStatusReturnsErrorDetailsWhenInValidRoadIdIsProvided()
        {
            string roadName = "A332";
            string expectedError= "road id is not recognised";

            // Arrange
            var jsonResponse = "{\"$type\":\"Tfl.Api.Presentation.Entities.ApiError, Tfl.Api.Presentation.Entities\"," +
                "\"timestampUtc\":\"2023-10-20T19:02:51.3290761Z\"," +
                "\"exceptionType\":\"EntityNotFoundException\"," +
                "\"httpStatusCode\":404," +
                "\"httpStatus\":\"NotFound\"," +
                "\"relativeUri\":\"/Road/A332?app_id=Road&app_key=ae364329b2354454b721baf025af1fc8\"," +
                "\"message\":\"The following road id is not recognised: A332\"}";

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var httpClientMock = new Mock<HttpClient>();
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent(jsonResponse, System.Text.Encoding.UTF8, "application/json"),
                }).Verifiable();

            var httpClient = new HttpClient(httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri($"https://api.tfl.gov.uk/{roadName}/?app_id=x&aapp_key=y"),
            };

            httpClientFactoryMock.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var roadService = new RoadService(httpClientFactoryMock.Object);

            // Act
            var responseDTo = await roadService.GetRoadStatus($"https://api.tfl.gov.uk/{roadName}/?app_id=x&aapp_key=y");


            // Assert
            Assert.IsTrue(responseDTo.Message.Contains(expectedError));
            Assert.IsTrue(responseDTo.Message.Contains(roadName));
            Assert.IsTrue(responseDTo.HttpStatusCode.Equals(HttpStatusCode.NotFound));
        }
    }
}