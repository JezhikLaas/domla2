using Ice;
using Microsoft.Extensions.Logging;

namespace D2.Service.ServiceProvider
{
    public class IceLogger : Ice.Logger
    {
        ILogger<IceLogger> _worker;

        string _prefix;

        internal IceLogger(ILogger<IceLogger> worker)
        {
            _worker = worker;
            _prefix = "IceLogger";
        }
        public Logger cloneWithPrefix(string prefix)
        {
            var result = new IceLogger(_worker);
            result._prefix = prefix;

            return result;
        }

        public void error(string message)
        {
            _worker.LogError(message);
        }

        public string getPrefix()
        {
            return _prefix;
        }

        public void print(string message)
        {
            _worker.LogInformation(message);
        }

        public void trace(string category, string message)
        {
            _worker.LogTrace(message);
        }

        public void warning(string message)
        {
            _worker.LogWarning(message);
        }
    }
}