using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Ninject;
using Ninject.Extensions.Conventions;
using System;
using System.Data;

namespace D2.MasterData.Infrastructure.IoC
{
    public static class DependencyResolver
    {
        static IKernel _kernel;

        internal static IApplicationBuilder RegisterApplicationComponents(this IApplicationBuilder app)
        {
            _kernel = new StandardKernel();

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
        }

        static internal object Resolve(Type type)
        {
            return _kernel.Get(type);
        }
    }
}
