using D2.Service.IoC;
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

        public bool ServiceReady
        {
            get;
            private set;
        }

        internal ServiceHost(DependencyResolver dependencyResolver)
        {
            _dependencyResolver = dependencyResolver;
        }

        static public IServiceHostBuilder CreateDefaultBuilder()
        {
            var dependencyResolver = new DependencyResolver();
            SetupLogging(dependencyResolver);

            var result = new ServiceHostBuilder(dependencyResolver);
            dependencyResolver.Kernel.Bind<IServiceHostBuilder>().ToConstant(result);
            dependencyResolver.Kernel.Bind<IServices>().ToConstant(dependencyResolver);

            return result;
        }

        static void SetupLogging(DependencyResolver dependencyResolver)
        {
            dependencyResolver.Kernel.Bind<ILoggerFactory>().To<LoggerFactory>().InSingletonScope();
            dependencyResolver.Kernel.Bind(typeof(ILogger<>)).To(typeof(Logger<>)).InSingletonScope();

            var factory = dependencyResolver.Kernel.Get<ILoggerFactory>();
            factory.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });

            if (File.Exists("nlog.config")) factory.ConfigureNLog("nlog.config");
        }

        public T Resolve<T>()
        {
            return _dependencyResolver.Resolve<T>();
        }

        public void Run()
        {
            var initializationData = new InitializationData();
            initializationData.logger = new IceLogger(_dependencyResolver.Resolve<ILogger<IceLogger>>());

            var configuration = _dependencyResolver.Resolve<IConfiguration>();
            var iceSection = configuration.GetSection("Ice");
            initializationData.properties = Util.createProperties();

            foreach (var pair in iceSection.AsEnumerable().Where(kvp => kvp.Value != null)) {
                initializationData.properties.setProperty(pair.Key.Substring(4).Replace(':', '.'), pair.Value);
            }

            using (var service = new IceService(_dependencyResolver, Environment.GetCommandLineArgs(), initializationData)) {
                service.Run(() => ServiceReady = true);
            }
        }
    }
}