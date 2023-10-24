using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TFLCodingChallenge.Options;
using TFLCodingChallenge.Service;

namespace TFLCodingChallenge
{
    class Program
    {
        public static string? roadName;

        static async Task Main(string[] args)
        {
            Console.Write("Please enter a RoadName: "); 
            roadName = Console.ReadLine();

            if(string.IsNullOrWhiteSpace(roadName))
            {
                Console.Write("Please enter some text...");
                return;
            }

            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            IServiceCollection serviceCollection = new ServiceCollection();

            ConfigureServices(serviceCollection, config);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            args = new string[] {roadName};
            await serviceProvider.GetService<TFLClientRunner>().RunAsync(args);
            Console.Read();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            // AppSettings IOptions configuration
            serviceCollection.AddOptions<TFLClientOptions>().Bind(configuration.GetSection(nameof(TFLClientOptions)));
            serviceCollection.AddHttpClient();
            serviceCollection.AddTransient<TFLClientRunner>();
            serviceCollection.AddTransient<IRoadService, RoadService>();
            serviceCollection.AddLogging();
        }
    }
}