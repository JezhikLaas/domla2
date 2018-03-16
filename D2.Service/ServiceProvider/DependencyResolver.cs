using D2.Service.Controller;
using Ninject;
using Ninject.Extensions.Conventions;
using System;
using System.Data;
using System.Linq;
using System.Reflection;

namespace D2.Service.ServiceProvider
{
    public interface IServices
    {
        IServices AddSingleton<TInterface>(TInterface instance);
        IServices Add<TInterface, TImplementation>()
                  where TImplementation : TInterface;
        IServices AddControllers();
    }

    public class DependencyResolver : IServices
    {
        public DependencyResolver()
        {
            Kernel = new StandardKernel();
        }

        internal IKernel Kernel
        {
            get;
            private set;
        }

        public void RegisterApplicationComponents(Assembly bindingSource, params Assembly[] bindingSources)
        {
            var sources = new Assembly[] { bindingSource }.Concat(bindingSources);

            Kernel.Bind(
                x => x.From(sources)
                      .SelectAllClasses()
                      .InheritedFrom(typeof(BaseController))
                      .WithoutAttribute<TopicAttribute>()
                      .BindBase()
                      .Configure((b, c) => {
                          var name = c.Name;
                          if (name.EndsWith("Controller")) name = name.Remove(name.Length - "Controller".Length);
                          b.Named(name);
                      })
            );

            Kernel.Bind(
                x => x.From(sources)
                      .SelectAllClasses()
                      .InheritedFrom(typeof(BaseController))
                      .WithAttribute<TopicAttribute>()
                      .BindBase()
                      .Configure((b, c) => {
                          var name = c.GetCustomAttribute<TopicAttribute>().Topic;
                          b.Named(name);
                      })
            );
            

            /*/
            // Register application services
            foreach (var ctrlType in app.GetControllerTypes()) {
                _kernel.Bind(ctrlType).ToSelf().InScope(Scope.RequestScope);
            }

            // This is where our bindings are configurated
            _kernel.Bind(
                x => x.FromThisAssembly()
                      .SelectAllClasses()
                      .WithAttribute<Singleton>()
                      .BindAllInterfaces()
                      .Configure(
                           b => b.InSingletonScope()
                       )
            );
            _kernel.Bind(
                x => x.FromThisAssembly()
                      .SelectAllClasses()
                      .WithAttribute<RequestScope>()
                      .BindAllInterfaces()
                      .Configure(
                           b => b.InScope(Scope.RequestScope)
                       )
            );
            _kernel.Bind(
                x => x.FromThisAssembly()
                      .SelectAllClasses()
                      .WithoutAttribute<RequestScope>()
                      .WithoutAttribute<Singleton>()
                      .BindAllInterfaces()
            );

            // Cross-wire required framework services
            _kernel.BindToMethod(app.GetRequestService<IViewBufferScope>);

            // Use factory pattern for connections
            _kernel.BindToMethod<IDbConnection>(ConnectionFactory.CreateConnection);

            return app;
            */
        }

        internal T Resolve<T>()
        {
            return Kernel.Get<T>();
        }

        internal T ResolveNamed<T>(string name)
        {
            return Kernel.Get<T>(name);
        }

        internal object Resolve(Type clazz)
        {
            return Kernel.Get(clazz);
        }

        internal object ResolveNamed(Type clazz, string name)
        {
            return Kernel.Get(clazz, name);
        }

        public IServices Add<TInterface, TImplementation>()
                         where TImplementation : TInterface
        {
            Kernel.Bind<TInterface>().To<TImplementation>();
            return this;
        }

        public IServices AddSingleton<TInterface>(TInterface instance)
        {
            Kernel.Bind<TInterface>().ToConstant(instance);
            return this;
        }

        public IServices AddSingleton<TInterface, TImplementation>()
                         where TImplementation : TInterface
        {
            Kernel.Bind<TInterface>()
                .To<TImplementation>()
                .InSingletonScope();
            return this;
        }

        public IServices AddControllers()
        {
            RegisterApplicationComponents(Assembly.GetCallingAssembly());
            return this;
        }
    }
}
