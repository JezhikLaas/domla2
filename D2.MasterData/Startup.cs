using D2.MasterData.Infrastructure;
using D2.Service.IoC;
using D2.Service.ServiceProvider;
using Microsoft.Extensions.Logging;
using System;
using System.Data;

namespace D2.MasterData
{
    public class Startup
    {
        ILogger<Startup> _logger;

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
                .AddControllers();

            _logger.LogDebug("finished 'ConfigureServices'");
        }

        public void Configure()
        {
        }
    }
}
