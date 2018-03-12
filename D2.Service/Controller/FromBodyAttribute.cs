using System;

namespace D2.Service.Controller
{
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    public sealed class FromBodyAttribute : System.Attribute
    {
        public FromBodyAttribute()
        { }
    }
}