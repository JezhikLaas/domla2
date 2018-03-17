using D2.Service.IoC;
using D2.Service.ServiceProvider;
using Microsoft.Extensions.Logging;
using System;

namespace D2.MasterData
{
    public class StartupIce
    {
        ILogger<StartupIce> _logger;

        public StartupIce(ILogger<StartupIce> logger)
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
            services.AddControllers();
        }

        public void Configure()
        {
        }
    }
}
