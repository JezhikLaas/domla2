using D2.Service.Controller;
using D2.Service.IoC;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace D2.Service.CallDispatcher
{
    public class Dispatcher
    {
        Dictionary<string, Func<string, IEnumerable<QueryParameter>, object>> _callCache;
        DependencyResolver _dependencyResolver;

        public Dispatcher(DependencyResolver dependencyResolver)
        {
            _dependencyResolver = dependencyResolver;
            _callCache = new Dictionary<string, Func<string, IEnumerable<QueryParameter>, object>>();
        }

        public object Call(string topic, string verb, string action, string body, IEnumerable<QueryParameter> arguments)
        {
            var controller = _dependencyResolver.ResolveNamed<BaseController>(topic);
            var call = GetCall(controller, verb, action, body, arguments);

            return call(body, arguments);
        }

        public object PreCallCheck(string topic, string verb, string action, string body, IEnumerable<QueryParameter> arguments)
        {
            var controller = _dependencyResolver.ResolveNamed<BaseController>(topic);
            var call = GetCall(controller, verb, "Validate_" + action, body, arguments);

            return call(body, arguments);
        }

        Func<string, IEnumerable<QueryParameter>, object> GetCall(BaseController controller, string verb, string action, string body, IEnumerable<QueryParameter> arguments)
        {
            var key = $"{controller.GetType().FullName}_{action}_{string.Join("_", arguments.Select(arg => arg.Name))}";
            lock (_callCache) {
                Func<string, IEnumerable<QueryParameter>, object> result;

                if (_callCache.TryGetValue(key, out result)) return result;

                var target = FindCallTarget(controller.GetType(), verb, action, body, arguments);
                if (target == null) throw new MissingMethodException($"Matching method for {action} not found");

                result = (extra, parameters) => {
                    var actualArguments = new List<object>();
                    foreach (var parameter in target.GetParameters()) {
                        if (parameter.GetCustomAttributes(false).Any(attribute => attribute.GetType() == typeof(FromBodyAttribute))) {
                            actualArguments.Add(JsonConvert.DeserializeObject(extra, parameter.ParameterType));
                        }
                        else {
                            var argument = parameters.First(arg => arg.Name == parameter.Name);
                            actualArguments.Add(Convert.ChangeType(argument.Value, parameter.ParameterType));
                        }
                    }

                    return target.Invoke(controller, actualArguments.ToArray());
                };

                _callCache.Add(key, result);

                return result;
            }
        }

        static MethodInfo FindCallTarget(Type controller, string verb, string action, string body, IEnumerable<QueryParameter> arguments)
        {
            var annotatedMethods = from info in controller.GetMethods()
                                   where info.GetCustomAttributes(false).OfType<RoutingAttribute>().Any()
                                         &&
                                         info.GetCustomAttributes(false).OfType<RoutingAttribute>().First().Action == action
                                         &&
                                         info.GetCustomAttributes(false).OfType<RoutingAttribute>().First().Verb == verb
                                   select info;

            var automaticMethods = from info in controller.GetMethods()
                                   where info.GetCustomAttributes(false).OfType<RoutingAttribute>().Any() == false
                                         &&
                                         info.Name == verb + action
                                   select info;

            var candidates = annotatedMethods.Concat(automaticMethods).ToList();

            foreach (var candidate in candidates) {
                var parameters = (
                    from parameter in candidate.GetParameters()
                    where parameter.GetCustomAttributes(false).OfType<FromBodyAttribute>().Any() == false
                    select parameter
                ).ToList();

                if (parameters.Any(parameter => arguments.Any(argument => argument.Name == parameter.Name) == false)) continue;

                return candidate;
            }

            return null;
        }
    }
}