using Ice;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Ninject;
using NLog.Extensions.Logging;
using System;
using System.IO;
using System.Linq;

namespace D2.Service.ServiceProvider
{

    public class ServiceHost : IServiceHost
    {
        DependencyResolver _dependencyResolver;

        internal ServiceHost(DependencyResolver dependencyResolver)
        {
            _dependencyResolver = dependencyResolver;
        }

        static public IServiceHostBuilder CreateDefaultBuilder()
        {
            var dependencyResolver = new DependencyResolver();
            SetupConfiguration(dependencyResolver);
            SetupLogging(dependencyResolver);

            var result = new ServiceHostBuilder(dependencyResolver);
            dependencyResolver.Kernel.Bind<IServiceHostBuilder>().ToConstant(result);

            return result;
        }

        static void SetupConfiguration(DependencyResolver dependencyResolver)
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", true)
                            .AddEnvironmentVariables();

            dependencyResolver.Kernel.Bind<IConfiguration>().ToConstant(builder.Build());
        }

        static void SetupLogging(DependencyResolver dependencyResolver)
        {
            dependencyResolver.Kernel.Bind<ILoggerFactory>().To<LoggerFactory>().InSingletonScope();
            dependencyResolver.Kernel.Bind(typeof(ILogger<>)).To(typeof(Logger<>)).InSingletonScope();

            var factory = dependencyResolver.Kernel.Get<ILoggerFactory>();
            factory.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });

            if (File.Exists("nlog.config")) factory.ConfigureNLog("nlog.config");
        }

        public void Run()
        {
            var initializationData = new InitializationData();
            initializationData.logger = new IceLogger(_dependencyResolver.Resolve<ILogger<IceLogger>>());

            var configuration = _dependencyResolver.Resolve<IConfiguration>();
            var iceSection = configuration.GetSection("Ice");

            foreach (var pair in iceSection.AsEnumerable().Where(kvp => kvp.Value != null)) {
                initializationData.properties.setProperty(pair.Key.Substring(4).Replace(':', '.'), pair.Value);
            }
            
            var service = new IceService(_dependencyResolver);
            service.main(Environment.GetCommandLineArgs(), initializationData);
        }
    }
}