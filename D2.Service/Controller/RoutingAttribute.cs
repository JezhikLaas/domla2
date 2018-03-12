using System;

namespace D2.Service.Controller
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class RoutingAttribute : Attribute
    {
        readonly string _action;
        
        // This is a positional argument
        public RoutingAttribute(string action)
        {
            _action = action;
        }
        
        public string Action
        {
            get { return _action; }
        }
    }
}
