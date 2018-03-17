using Ninject.Activation;
using Ninject.Infrastructure.Disposal;
using System;
using System.Threading;

namespace D2.Service.IoC
{
    sealed class Scope : DisposableObject
    {
        static readonly AsyncLocal<Scope> scopeProvider = new AsyncLocal<Scope>();

        static internal Scope RequestScope(IContext context) => scopeProvider.Value;
        static internal IDisposable BeginScope()
        {
            scopeProvider.Value = new Scope();
            return scopeProvider.Value;
        }
    }
}
