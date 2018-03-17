using System;

namespace D2.Service.IoC
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class TransientAttribute : Attribute
    { }
}
