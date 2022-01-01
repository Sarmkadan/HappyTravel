using HappyTravel.Edo.Api.Infrastructure.Environments;
using HappyTravel.SentryLogger.Extensions;
using HappyTravel.StdOutLogger.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HappyTravel.Edo.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }


        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var environment = hostingContext.HostingEnvironment;

                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.ClearProviders()
                        .AddConfiguration(hostingContext.Configuration.GetSection("Logging"));

                    var env = hostingContext.HostingEnvironment;
                    if (env.IsLocal())
                        logging.AddConsole();
                    else
                    {
                        logging.AddStdOutLogger(setup =>
                        {
                            setup.IncludeScopes = false;
                            setup.RequestIdHeader = Infrastructure.Constants.Common.RequestIdHeader;
                            setup.UseUtcTimestamp = true;
                        });
                        logging.AddEventSourceLogger()
                            .AddSentry(c => { c.Endpoint = EnvironmentVariableHelper.Get("Logging:Sentry:Endpoint", hostingContext.Configuration); });
                    }
                })
                .UseSetting(WebHostDefaults.SuppressStatusMessagesKey, "true");
    }
}
