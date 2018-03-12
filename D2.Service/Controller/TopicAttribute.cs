using System;

namespace D2.Service.Controller
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class TopicAttribute : Attribute
    {
        readonly string _topic;
        
        public TopicAttribute(string topic)
        {
            _topic = topic;
        }
        
        public string Topic
        {
            get { return _topic; }
        }
    }
}
