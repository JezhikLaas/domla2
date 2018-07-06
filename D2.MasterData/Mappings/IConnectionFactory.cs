using FluentNHibernate.Cfg;
using NHibernate;

namespace D2.MasterData.Mappings {
    public interface IConnectionFactory {
        void Initialize();
        void Initialize(FluentConfiguration configuration);
        void Shutdown();
        ISession Open();
        ISession OpenReadOnly();
        ISession Open(string key);
        ISession OpenReadOnly(string key);
    }
}