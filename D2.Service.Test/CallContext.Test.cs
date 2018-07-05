using System.Collections.Generic;
using D2.Service.Controller;
using Xunit;

namespace D2.Service.Test
{
    public class CallContextTest
    {
        [Fact(DisplayName = "Call context returns null for unknown keys")]
        public void ContextReturnsNullForUnknownKeys()
        {
            var target = new CallContext();
            Assert.Null(target["xxx"]);
        }
        
        [Fact(DisplayName = "Call context returns value for known keys")]
        public void ContextReturnsValueForKnownKeys()
        {
            var target = new CallContext();
            target.SetupContext(new Dictionary<string, string>(){ {"value", "test"} });
            Assert.Equal("test", target["value"]);
        }
        
        [Fact(DisplayName = "Call context is case insensitive")]
        public void ContextIsCaseInsensitive()
        {
            var target = new CallContext();
            target.SetupContext(new Dictionary<string, string>(){ {"value", "test"} });
            Assert.Equal("test", target["Value"]);
        }
    }
}