using D2.Service.IoC;
using Microsoft.Extensions.Configuration;
using Ninject;
using System;
using System.Collections.Generic;
using System.IO;

namespace D2.Service.ServiceProvider
{
    public interface IServiceHostBuilder
    {
        IServiceHostBuilder UseStartup<T>();
        IServiceHostBuilder UseConfiguration(Action<IConfigurationBuilder> options);
        IServiceHost Build();
    }

    public class ServiceHostBuilder : IServiceHostBuilder
    {
        DependencyResolver _dependencyResolver;
        Type _startupType;

        Action<IConfigurationBuilder> _configurationOptions;

        internal ServiceHostBuilder(DependencyResolver dependencyResolver)
        {
            _dependencyResolver = dependencyResolver;
        }

        public IServiceHostBuilder UseStartup<T>()
        {
            if (_startupType != null) throw new InvalidOperationException("startup type has already been set, do not invoke 'UseStartup' twice");

            _startupType = typeof(T);
            _dependencyResolver.Kernel.Bind(_startupType).ToSelf();

            return this;
        }

        public IServiceHostBuilder UseConfiguration(Action<IConfigurationBuilder> options)
        {
            _configurationOptions = options;
            return this;
        }

        void SetupConfiguration()
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", true)
                            .AddEnvironmentVariables();

            if (_configurationOptions != null) _configurationOptions(builder);

            _dependencyResolver.Kernel.Bind<IConfiguration>().ToConstant(builder.Build());
        }

        public IServiceHost Build()
        {
            if (_startupType == null) throw new InvalidOperationException("startup type has not been set, invoke 'UseStartup'");

            SetupConfiguration();

            var startup = _dependencyResolver.Kernel.Get(_startupType);

            var configureServices = _startupType.GetMethod("ConfigureServices", new[] { typeof(IServices) });
            if (configureServices == null) throw new MissingMethodException("Startup has to provide a ConfigureServices method, taking an IServices as parameter");

            configureServices.Invoke(startup, new[] { _dependencyResolver });

            var configure = _startupType.GetMethod("Configure");
            if (configure == null) throw new MissingMethodException("Startup has to provide a Configure method");

            var arguments = new List<object>();
            
            foreach (var parameter in configure.GetParameters()) {
                arguments.Add(_dependencyResolver.Kernel.Get(parameter.ParameterType));
            }

            configure.Invoke(startup, arguments.ToArray());

            return new ServiceHost(_dependencyResolver);
        }
    }
}