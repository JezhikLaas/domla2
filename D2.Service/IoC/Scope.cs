using Ninject.Activation;
using Ninject.Infrastructure.Disposal;
using System;
using System.Threading;

namespace D2.Service.IoC
{
    sealed class Scope : DisposableObject
    {
        static readonly AsyncLocal<Scope> ScopeProvider = new AsyncLocal<Scope>();

        internal static Scope RequestScope(IContext context) => ScopeProvider.Value;
        internal static IDisposable BeginScope()
        {
            ScopeProvider.Value = new Scope();
            return ScopeProvider.Value;
        }
    }
}
