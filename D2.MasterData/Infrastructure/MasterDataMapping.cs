using System.Collections.Generic;
using System.Reflection;
using D2.Common;
using D2.MasterData.Models;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using NHibernate;
using NHibernate.Event;
using Npgsql;

namespace D2.MasterData.Infrastructure
{
    public static class ConnectionFactory
    {
        private static ISessionFactory _sessionFactory;

        private static readonly object SyncRoot;

        private static void Initialize()
        {
            lock (SyncRoot)
            {
                if (_sessionFactory == null)
                {
                    var builder = new NpgsqlConnectionStringBuilder();
                    var options = ServiceConfiguration.connectionInfo;

                    builder.ApplicationName = options.Identifier;
                    builder.Database = options.Name;
                    builder.Host = options.Host;
                    builder.Password = options.Password;
                    builder.Username = options.User;
                    builder.Port = options.Port;
            
                    var connectionProperties = new Dictionary<string, string>
                    {
                        {"connection.connection_string", builder.ConnectionString },
                        { "connection.driver_class", "Beginor.NHibernate.NpgSql.NpgSqlDriver,NHibernate.NpgSql" },
                        { "dialect", "NHibernate.Dialect.PostgreSQL83Dialect" },
                        { "use_proxy_validator", "false" }
                    };

                    var configuration = new NHibernate.Cfg.Configuration()
                        .SetProperties(connectionProperties);
                    
                    Initialize(Fluently.Configure(configuration));
                }
            }
        }
        
        static ConnectionFactory()
        {
            SyncRoot = new object();
        }

        public static void Initialize(FluentConfiguration configuration)
        {
            _sessionFactory?.Dispose();

            _sessionFactory = configuration
                .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
                .BuildConfiguration()
                .BuildSessionFactory();
        }

        public static void Shutdown()
        {
            _sessionFactory?.Dispose();
        }
        
        public static ISession Open()
        {
            Initialize();
            return _sessionFactory
                .WithOptions()
                .ConnectionReleaseMode(ConnectionReleaseMode.OnClose)
                .NoInterceptor()
                .FlushMode(FlushMode.Auto)
                .OpenSession();
        }
        
        public static ISession OpenReadOnly()
        {
            Initialize();
            return _sessionFactory
                .WithOptions()
                .ConnectionReleaseMode(ConnectionReleaseMode.OnClose)
                .NoInterceptor()
                .FlushMode(FlushMode.Manual)
                .OpenSession();
        }
    }
    
    public class AdministrationUnitMap : ClassMap<AdministrationUnit>
    {
        public AdministrationUnitMap()
        {
            Table("administrationunits");
            Id(x => x.Id);
            Version(x => x.Version)
                .Column("xmin")
                .Generated.Always();
            Map(x => x.UserKey)
                .Access.BackingField()
                .Length(10)
                .Not
                .Nullable();
            Map(x => x.Edit)
                .Access.BackingField()
                .Generated.Always();
            Map(x => x.Title)
                .Access.BackingField()
                .Length(256)
                .Nullable();
            Map(x => x.YearOfConstruction)
                .Access.BackingField()
                .Nullable();
            HasMany(x => x.Entrances)
                .Cascade
                .AllDeleteOrphan()
                .Inverse();
        }
    }

    public class EntranceMap : ClassMap<Entrance>
    {
        public EntranceMap()
        {
            Table("entrances");
            Id(x => x.Id);
            Version(x => x.Version)
                .Column("xmin")
                .Generated.Always();
            Map(x => x.Edit)
                .Access.BackingField()
                .Generated.Always();
            Map(x => x.Title)
                .Access.BackingField()
                .Length(256)
                .Nullable();
            References(x => x.AdministrationUnit)
                .Access.BackingField()
                .Not.Nullable();
            Component(x => x.Address);
            HasMany(x => x.SubUnits)
                .Cascade.AllDeleteOrphan()
                .Inverse();
        }
    }

    public class SubUnitMap : ClassMap<SubUnit>
    {
        public SubUnitMap()
        {
            Table("subunits");
            Id(x => x.Id);
            Version(x => x.Version)
                .Column("xmin")
                .Generated.Always();
            Map(x => x.Edit)
                .Access.BackingField()
                .Generated.Always();
            Map(x => x.Title)
                .Access.BackingField()
                .Length(256)
                .Nullable();
            Map(x => x.Floor)
                .Access.BackingField()
                .Nullable();
            Map(x => x.Number)
                .Access.BackingField()
                .Not.Nullable();
            Map(x => x.Usage)
                .Access.BackingField()
                .Length(256)
                .Nullable();
            References(x => x.Entrance)
                .Access.BackingField();
        }
    }

    public class AddressMap : ComponentMap<Address>
    {
        public AddressMap()
        {
            Map(x => x.City)
                .Access.BackingField()
                .Length(100);
            Map(x => x.Number)
                .Access.BackingField()
                .Length(10);
            Map(x => x.PostalCode)
                .Access.BackingField()
                .Length(20);
            Map(x => x.Street)
                .Access.BackingField()
                .Length(150);
            Component(c => c.Country, b =>
            {
                b.Map(x => x.Iso2)
                    .Access.BackingField()
                    .Length(3);
                b.Map(x => x.Iso3)
                    .Access.BackingField()
                    .Length(4);
                b.Map(x => x.Name)
                    .Access.BackingField()
                    .Length(100);
            });
        }
    }
}