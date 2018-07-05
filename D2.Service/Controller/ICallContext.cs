using System.Collections.Generic;

namespace D2.Service.Controller {
    public interface ICallContext
    {
        void SetupContext(IDictionary<string, string> values);
        string this[string key] { get; }
        IEnumerable<KeyValuePair<string, string>> Context { get; }
    }
}