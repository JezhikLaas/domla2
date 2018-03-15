using D2.Service.CallDispatcher;
using Ice;

namespace D2.Service.ServiceProvider
{
    public class IceService : Application
    {
        Dispatcher _dispatcher;

        public IceService(DependencyResolver dependencyResolver)
        {
            _dispatcher = new Dispatcher(dependencyResolver);
        }

        public override int run(string[] args)
        {
            //Service.communicator().
            return 0;
        }
    }
}