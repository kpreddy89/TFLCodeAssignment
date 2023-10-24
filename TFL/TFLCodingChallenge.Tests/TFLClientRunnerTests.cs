using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Net;
using TFLCodingChallenge.DTOs;
using TFLCodingChallenge.Options;
using TFLCodingChallenge.Service;

namespace TFLCodingChallenge.Tests
{
    [TestClass]
    public class TFLClientRunnerTests
    {
        private Mock<IOptions<TFLClientOptions>> clientOptionsMock;
        private IServiceProvider serviceProvider;
        private Mock<IRoadService> roadServiceMock;

        [TestInitialize]
        public void Initialize()
        {
            clientOptionsMock = new Mock<IOptions<TFLClientOptions>>();
            roadServiceMock = new Mock<IRoadService>();

             serviceProvider = new ServiceCollection()
             .AddTransient<IRoadService, RoadService>()
             .BuildServiceProvider();
        }

        [TestMethod]
        public async Task RunAsync_ValidInput_ExitCode_Equals_Zero()
        {
            // Arrange
            var args = new string[] { "A40" };
            var responseDto = new ResponseDto(HttpStatusCode.OK, "OK")
            {
                RoadDetails = new RoadDto()
                {
                    DisplayName = "A40",
                    StatusSeverity = "Good",
                    StatusSeverityDescription = "No Exceptional Delays"
                }
                
            };

            clientOptionsMock.Setup(x => x.Value).Returns(new TFLClientOptions
            {
                Endpoint = "https://api.tfl.gov.uk/Road/",
                App_Id = "your_app_id",
                App_Key = "your_developer_key"
            });

            roadServiceMock.Setup(x => x.GetRoadStatus(It.IsAny<string>())).ReturnsAsync(responseDto);


            var serviceProviderMock = new Mock<MockServiceProvider>(serviceProvider, roadServiceMock);;

            var runner = new TFLClientRunner(clientOptionsMock.Object, serviceProviderMock.Object);

            var expectedOutput = $"The status of the A40 is as follows:  \r\nRoad Status is Good \r\nRoad Status Description is No Exceptional Delays \r\n";

            var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            // Act
            await runner.RunAsync(args);

            // Assert
            var output = consoleOutput.ToString();
            Assert.AreEqual(expectedOutput.TrimEnd(), output.TrimEnd());
            Assert.AreEqual(runner.exitCode, 0);
        }

        [TestMethod]
        public async Task RunAsync_InvalidInput_ExitCode_Equals_One()
        {
            // Arrange
            var args = new string[] { "A55" };
            var responseDto = new ResponseDto(HttpStatusCode.NotFound, "The following road id is not recognised: A55");
 

            clientOptionsMock.Setup(x => x.Value).Returns(new TFLClientOptions
            {
                Endpoint = "https://api.tfl.gov.uk/Road/",
                App_Id = "your_app_id",
                App_Key = "your_developer_key"
            });

            roadServiceMock.Setup(x => x.GetRoadStatus(It.IsAny<string>())).ReturnsAsync(responseDto);


            var serviceProviderMock = new Mock<MockServiceProvider>(serviceProvider, roadServiceMock); ;

            var runner = new TFLClientRunner(clientOptionsMock.Object, serviceProviderMock.Object);

            var expectedOutput = $"A55 is not a valid road \r\n";

            var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            // Act
            await runner.RunAsync(args);

            // Assert
            var output = consoleOutput.ToString();
            Assert.AreEqual(expectedOutput.TrimEnd(), output.TrimEnd());
            Assert.AreEqual(runner.exitCode, 1);
        }
    }
}
