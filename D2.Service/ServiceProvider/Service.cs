using D2.Service.CallDispatcher;
using Ice;
using Microsoft.Extensions.Logging;

namespace D2.Service.ServiceProvider
{
    public interface IServive
    {
        void Shutdown();
    }

    public class IceService : Application, IServive
    {
        DependencyResolver _dependencyResolver;
        Dispatcher _dispatcher;

        public IceService(DependencyResolver dependencyResolver)
        {
            _dependencyResolver.AddSingleton<IServive>(this);
            _dependencyResolver = dependencyResolver;
            _dispatcher = new Dispatcher(dependencyResolver);
        }

        public override int run(string[] args)
        {
            try {
                var validator = communicator().createObjectAdapter("Validator");
                validator.add(
                    new Validator(_dependencyResolver.Resolve<ILogger<Validator>>(), _dispatcher),
                    Util.stringToIdentity("Validator"));

                communicator().waitForShutdown();
                return 0;
            }
            catch (Exception error) {
                communicator().getLogger().error(error.ToString());
                return 1;
            }
        }

        public void Shutdown()
        {
            communicator().shutdown();
        }
    }
}