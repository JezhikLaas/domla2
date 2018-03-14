using Ninject;
using System;
using System.Collections.Generic;

namespace D2.Service.ServiceProvider
{
    public interface IServiceHostBuilder
    {
        IServiceHostBuilder UseStartup<T>();
        IServiceHost Build();
    }

    public class ServiceHostBuilder : IServiceHostBuilder
    {
        DependencyResolver _dependencyResolver;
        Type _startupType;

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

        public IServiceHost Build()
        {
            if (_startupType == null) throw new InvalidOperationException("startup type has not been set, invoke 'UseStartup'");

            var startup = _dependencyResolver.Kernel.Get(_startupType);

            var configureServices = _startupType.GetMethod("ConfigureServices", new[] { typeof(IKernel) });
            if (configureServices == null) throw new MissingMethodException("Startup has to provide a ConfigureServices method, taking a Ninject.IKernel as parameter");

            configureServices.Invoke(startup, new[] { _dependencyResolver.Kernel });

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