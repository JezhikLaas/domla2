using D2.Service.CallDispatcher;
using D2.Service.IoC;
using Ice;
using Microsoft.Extensions.Logging;
using System;

namespace D2.Service.ServiceProvider
{
    public interface IService
    {
        void Shutdown();
    }

    public class IceService : IService, IDisposable
    {
        DependencyResolver _dependencyResolver;
        Dispatcher _dispatcher;
        Ice.Communicator _communicator;

        public IceService(DependencyResolver dependencyResolver, string[] args, InitializationData initializationData)
        {
            _dependencyResolver = dependencyResolver;
            _dependencyResolver.AddSingleton<IService>(this);
            _dispatcher = new Dispatcher(dependencyResolver);
            _communicator = Ice.Util.initialize(ref args, initializationData);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public int Run(Action enterIdle)
        {
            try {
                var adapter = _communicator.createObjectAdapter("Dispatcher");
                adapter.add(
                    new Validator(_dependencyResolver.Resolve<ILogger<Validator>>(), _dispatcher),
                    Util.stringToIdentity("Validator"));
                adapter.add(
                    new Executor(_dependencyResolver.Resolve<ILogger<Executor>>(), _dispatcher),
                    Util.stringToIdentity("Executor"));

                adapter.activate();

                enterIdle();
                _communicator.waitForShutdown();

                return 0;
            }
            catch (System.Exception error) {
                _communicator.getLogger().error(error.ToString());
                return 1;
            }
        }

        public void Shutdown()
        {
            if (_communicator != null) _communicator.shutdown();
        }

        protected void Dispose(bool disposing)
        {
            if (disposing) {
                if (_communicator.isShutdown() == false) {
                    _communicator.shutdown();
                }
                _communicator.Dispose();
                _communicator = null;
            }
        }
    }
}