using Ninject;

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

            var result = new ServiceHostBuilder(dependencyResolver);
            dependencyResolver.Kernel.Bind<IServiceHostBuilder>().ToConstant(result);

            return result;
        }

        public void Run()
        {
            ;
        }
    }
}