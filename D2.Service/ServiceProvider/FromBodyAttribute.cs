using System;

namespace D2.Service.ServiceProvider
{
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    public sealed class FromBodyAttribute : System.Attribute
    {
        public FromBodyAttribute()
        { }
    }
}