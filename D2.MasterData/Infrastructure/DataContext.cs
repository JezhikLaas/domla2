using System;
using D2.MasterData.Mappings;
using D2.Service.Controller;
using D2.Service.IoC;
using NHibernate;

namespace D2.MasterData.Infrastructure
{
    [RequestScope]
    public class DataContext : IDataContext
    {
        public DataContext(ICallContext context, IConnectionFactory factory)
        {
            var key = context["dbkey"];
            Session = factory.Open(key);
        }
        
        public ISession Session { get; }

        public void Dispose()
        {
            Dispose(true);
        }

        protected void Dispose(bool disposing)
        {
            if (!disposing) return;
            Session?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}