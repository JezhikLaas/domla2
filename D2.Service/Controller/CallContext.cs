using System.Collections.Generic;
using D2.Service.IoC;

namespace D2.Service.Controller
{
    [RequestScope]
    public class CallContext : ICallContext
    {
        public CallContext()
        {
            _values = new Dictionary<string, string>();
        }
        
        private readonly Dictionary<string, string> _values;

        public void SetupContext(IDictionary<string, string> values)
        {
            foreach (var value in values) {
                _values.Add(value.Key.ToLowerInvariant(), value.Value);
            }
        }

        public string this[string key] =>
            _values.ContainsKey(key.ToLowerInvariant()) ? _values[key.ToLowerInvariant()] : null;
        
        public IEnumerable<KeyValuePair<string, string>> Context => _values;
    }
}