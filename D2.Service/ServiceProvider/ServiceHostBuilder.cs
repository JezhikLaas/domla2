using System;

namespace D2.Service.ServiceProvider
{
    public class ServiceHostBuilder
    {
        Type _startupType;
        public ServiceHostBuilder UseStartup<T>()
        {
            _startupType = typeof(T);
            return this;
        }

        public IServiceHost Build()
        {
            if (_startupType == null) throw new InvalidOperationException("startup type has not been set, invoke 'UseStartup'");
            return null;
        }
    }
}