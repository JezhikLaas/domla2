using D2.Service.IoC;
using Microsoft.Extensions.Logging;
using System;
using D2.MasterData.Mappings;

namespace D2.MasterData
{
    public class Startup
    {
        readonly ILogger<Startup> _logger;

        public Startup(ILogger<Startup> logger)
        {
            _logger = logger;

            if (ServiceRegistration.registerSelf(logger) == false) {
                logger.LogCritical("failed to register self");
                throw new InvalidOperationException("failed to register self");
            }
        }

        public void ConfigureServices(IServices services)
        {
            _logger.LogDebug("starting 'ConfigureServices'");

            services
                .AddControllers()
                ;

            _logger.LogDebug("finished 'ConfigureServices'");
        }

        public void Configure(IServices services)
        {
            try {
                var factory = services.Resolve<IConnectionFactory>();
                factory.Initialize();
                _logger.LogInformation("Database initialized");
            }
            catch (Exception error) {
                _logger.LogCritical(error, "DB initialization failed");
                throw;
            }
        }
    }
}
