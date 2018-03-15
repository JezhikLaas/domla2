using Ice;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Ninject;
using NLog.Extensions.Logging;
using System;
using System.IO;

namespace D2.Service.ServiceProvider
{
    public class Service : Application
    {
        public override int run(string[] args)
        {
            //Service.communicator().
            return 0;
        }
    }

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
            factory.ConfigureNLog("nlog.config");
        }

        public void Run()
        {
            InitializationData initializationData = new InitializationData();
            var service = new Service();
            service.main(Environment.GetCommandLineArgs(), initializationData);
        }
    }
}