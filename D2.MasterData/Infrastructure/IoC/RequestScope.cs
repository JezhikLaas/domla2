using System;

namespace D2.MasterData.Infrastructure.IoC
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class RequestScope : Attribute
    { }
}
