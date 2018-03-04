using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace D2.MasterData.Infrastructure.IoC
{
    public static class AspNetCoreExtensions
    {
        public static IServiceCollection AddRequestScopingMiddleware(
            this IServiceCollection services,
            Func<IDisposable> requestScopeProvider)
        {
            if (services == null) {
                throw new ArgumentNullException(nameof(services));
            }

            if (requestScopeProvider == null) {
                throw new ArgumentNullException(nameof(requestScopeProvider));
            }

            return services
                .AddSingleton<IStartupFilter>(new RequestScopingStartupFilter(requestScopeProvider));
        }

        public static IServiceCollection AddCustomControllerActivation(
            this IServiceCollection services,
            Func<Type, object> activator)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (activator == null) throw new ArgumentNullException(nameof(activator));

            return services.AddSingleton<IControllerActivator>(
                new DelegatingControllerActivator(
                    context => activator(context.ActionDescriptor.ControllerTypeInfo.AsType())
                )
            );
        }

        public static IServiceCollection AddCustomViewComponentActivation(
            this IServiceCollection services,
            Func<Type, object> activator)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (activator == null) throw new ArgumentNullException(nameof(activator));

            return services.AddSingleton<IViewComponentActivator>(
                new DelegatingViewComponentActivator(activator)
            );
        }
    }
}
