using D2.Service;
using D2.Service.ServiceProvider;
using Newtonsoft.Json;
using System;
using System.Reflection;
using Xunit;

namespace D2.Service.Test
{
    public class DispatcherTest
    {
        public class TestParameter
        {
            public int ValueOne { get; set; }
            public string ValueTwo { get; set; }
        }

        public class SimpleController : BaseController
        {
            public string CallReturnsString()
            {
                return "Hello world!";
            }

            public void CallReturnsVoid()
            { }

            public string CallTakingObject([FromBody]TestParameter param)
            {
                return param.ValueTwo;
            }

            public string CallTakingParameters(string first, string second)
            {
                return $"{first} {second}!";
            }
        }
    
        public DispatcherTest()
        {
            DependencyResolver.RegisterApplicationComponents(Assembly.GetExecutingAssembly());
        }
        
        [Fact(DisplayName = "Execute a simple call returning a string")]
        public void Execute_a_simple_call_returning_a_string()
        {
            Assert.Equal("Hello world!", Dispatcher.Call("Simple", "CallReturnsString", null, new QueryParameter[0]));
        }
        
        [Fact(DisplayName = "Execute a simple call returning void")]
        public void Execute_a_simple_call_returning_void()
        {
            Assert.Null(Dispatcher.Call("Simple", "CallReturnsVoid", null, new QueryParameter[0]));
        }
        
        [Fact(DisplayName = "Execute a call with a FromBody parameter")]
        public void Execute_a_call_with_a_frombody_parameter()
        {
            var parameter = new TestParameter { ValueOne = 1, ValueTwo = "Eins" };
            var parameterString = JsonConvert.SerializeObject(parameter);

            Assert.Equal("Eins", Dispatcher.Call("Simple", "CallTakingObject", parameterString, new QueryParameter[0]));
        }
        
        [Fact(DisplayName = "Execute a call with parameters")]
        public void Execute_a_call_with_parameters()
        {
            var parameters = new QueryParameter[] {
                new QueryParameter { Name = "first", Value = "Hello" },
                new QueryParameter { Name = "second", Value = "world" }
            };

            Assert.Equal("Hello world!", Dispatcher.Call("Simple", "CallTakingParameters", null, parameters));
        }
        
        [Fact(DisplayName = "Execute a call with different parameters yields different results")]
        public void Execute_a_call_with_different_parameters_yields_different_results()
        {
            var parametersOne = new QueryParameter[] {
                new QueryParameter { Name = "first", Value = "Hello" },
                new QueryParameter { Name = "second", Value = "world" }
            };

            var parametersTwo = new QueryParameter[] {
                new QueryParameter { Name = "first", Value = "Goodbye" },
                new QueryParameter { Name = "second", Value = "Hamburg" }
            };

            Assert.Equal("Hello world!", Dispatcher.Call("Simple", "CallTakingParameters", null, parametersOne));
            Assert.Equal("Goodbye Hamburg!", Dispatcher.Call("Simple", "CallTakingParameters", null, parametersTwo));
        }
    }
}
