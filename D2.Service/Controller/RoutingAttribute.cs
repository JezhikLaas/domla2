using System;

namespace D2.Service.Controller
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class RoutingAttribute : Attribute
    {
        readonly string _action;
        readonly string _verb;

        // This is a positional argument
        public RoutingAttribute(string verb, string action)
        {
            _verb = verb;
            _action = action;
        }

        public string Action => _action;
        public string Verb => _verb;
    }
}
