using Ninject;
using Ninject.Extensions.Conventions;
using System;
using System.Data;
using System.Reflection;

namespace D2.Service
{
    public static class DependencyResolver
    {
        static IKernel _kernel;

        static public void RegisterApplicationComponents(params Assembly[] bindingSources)
        {
            _kernel = new StandardKernel();

            _kernel.Bind(
                x => x.FromThisAssembly()
                      .SelectAllClasses()
                      .InheritedFrom(typeof(BaseController))
                      .Join
                      .From(bindingSources)
                      .SelectAllClasses()
                      .InheritedFrom(typeof(BaseController))
                      .BindBase()
                      .Configure((b, c) => {
                          var name = c.Name;
                          if (name.EndsWith("Controller")) name = name.Remove(name.Length - "Controller".Length);
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

        static internal T Resolve<T>()
        {
            return _kernel.Get<T>();
        }

        static internal T ResolveNamed<T>(string name)
        {
            return _kernel.Get<T>(name);
        }
    }
}
