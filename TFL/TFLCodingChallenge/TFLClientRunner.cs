using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TFLCodingChallenge.Options;
using TFLCodingChallenge.Service;

namespace TFLCodingChallenge
{
    public class TFLClientRunner
    {
        private readonly TFLClientOptions _clientOptions;
        private readonly ILogger<TFLClientRunner> _logger;
        private readonly IRoadService _roadService;
        public int exitCode { get; set; }

        public TFLClientRunner(IOptions<TFLClientOptions> clientOptions, IServiceProvider serviceProvider)
        {
            _clientOptions = clientOptions.Value;
            _logger = serviceProvider.GetService<ILogger<TFLClientRunner>>();
            _roadService = serviceProvider.GetRequiredService<IRoadService>();
        }

        public async Task RunAsync(string[] args)
        {
            try
            {
                var roadName = args[0]; 
                var responseDto = await _roadService.GetRoadStatus(_clientOptions.Endpoint+ roadName +
                    "?app_id=" + _clientOptions.App_Id+ "&app_key=" +_clientOptions.App_Key);

                if (responseDto != null)
                {
                    StringBuilder message = new();
                    switch (responseDto.HttpStatusCode)
                    {
                        case System.Net.HttpStatusCode.OK:
                            message.Append($"The status of the {responseDto.RoadDetails?.DisplayName} is as follows:  \r\n" +
                        $"Road Status is {responseDto.RoadDetails?.StatusSeverity} \r\n" +
                        $"Road Status Description is {responseDto.RoadDetails?.StatusSeverityDescription} \r\n");
                            exitCode = 0;
                            break;
                        case System.Net.HttpStatusCode.NotFound:
                            message.Append($"{roadName} is not a valid road");
                            exitCode = 1;
                            break;
                        default:
                            message.Append($"{responseDto.Message}");
                            exitCode = 1;
                            break;
                    }
                    Console.WriteLine(message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred..");
                //An error occured
                exitCode = 2;
            }
            Environment.ExitCode = exitCode;
        }
    }
}
