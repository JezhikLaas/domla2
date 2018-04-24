using System;
using NHibernate;

namespace D2.MasterData.Infrastructure
{
    public interface IDataContext : IDisposable
    {
        ISession Session { get; }
    }
}