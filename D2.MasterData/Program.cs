using D2.Common;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System.IO;

namespace D2.MasterData
{
    public class Program
    {
        public static int Main(string[] args)
        {
            try {
                var logConfig = Path.Combine(Directory.GetCurrentDirectory(), "nlog.config");
                if (File.Exists(logConfig)) {
                    var logger = NLog.LogManager.LoadConfiguration(logConfig).GetCurrentClassLogger();
                    logger.Info("logging configured");
                }
                BuildWebHost(args).Run();
                return 0;
            }
            catch {
                return 1;
            }
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(options => ServiceConfiguration.configureKestrel(options))
                .UseStartup<Startup>()
                .ConfigureLogging(
                    logging => {
                        logging.ClearProviders();
                        logging.SetMinimumLevel(LogLevel.Trace);
                    }
                )
                .UseNLog()
                .Build();
    }
}
