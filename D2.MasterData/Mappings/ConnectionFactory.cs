using System;
using System.Collections.Generic;
using D2.Common;
using FluentNHibernate.Cfg;
using Microsoft.FSharp.Core;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Npgsql;

namespace D2.MasterData.Mappings
{
    public static class ConnectionFactory
    {
        private static ISessionFactory _sessionFactory;

        private static readonly Cache<ISessionFactory, string> SessionFactories;

        private static readonly object SyncRoot;
        
        static ConnectionFactory()
        {
            SyncRoot = new object();
            Func<string, ISessionFactory> createFactory = x => CreateFactory(x);
            Func<ISessionFactory, Unit> dropFactory = x => DropFactory(x);
            
            var options = new CacheOptions<ISessionFactory, string>(
                FSharpFuncUtil<string, ISessionFactory>.ToFSharpFunc(createFactory),
                FSharpFuncUtil<ISessionFactory, Unit>.ToFSharpFunc(dropFactory),
                600,
                10
                );
            SessionFactories = new Cache<ISessionFactory, string>(options);
        }

        public static void Initialize()
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
                        { "connection.connection_string", builder.ConnectionString },
                        { "connection.driver_class", "Beginor.NHibernate.NpgSql.NpgSqlDriver,NHibernate.NpgSql" },
                        { "dialect", "NHibernate.Dialect.PostgreSQL83Dialect" }
                    };

                    var configuration = new NHibernate.Cfg.Configuration()
                        .SetProperties(connectionProperties);
                    
                    _sessionFactory = Fluently.Configure(configuration)
                        .Mappings(m =>
                            m.FluentMappings
                                .Add<AdministrationUnitMap>()
                                .Add<EntranceMap>()
                                .Add<SubUnitMap>()
                                .Add<AddressMap>()
                                .Add<PostalCodeInfoMap>()
                                .Add<AdministrationUnitsFeatureMap>()
                                .Add<AdministrationUnitPropertyMap>()
                        )
                        .ExposeConfiguration(BuildSchema)
                        .BuildConfiguration()
                        .BuildSessionFactory();
                }
            }
        }

        private static ISessionFactory CreateFactory(string key)
        {
            var builder = new NpgsqlConnectionStringBuilder();
            var options = ServiceConfiguration.connectionInfo;

            builder.ApplicationName = options.Identifier;
            builder.Database = key;
            builder.Host = options.Host;
            builder.Password = key;
            builder.Username = key;
            builder.Port = options.Port;
    
            var connectionProperties = new Dictionary<string, string>
            {
                { "connection.connection_string", builder.ConnectionString },
                { "connection.driver_class", "Beginor.NHibernate.NpgSql.NpgSqlDriver,NHibernate.NpgSql" },
                { "dialect", "NHibernate.Dialect.PostgreSQL83Dialect" }
            };

            var configuration = new NHibernate.Cfg.Configuration()
                .SetProperties(connectionProperties);
            
            return Fluently.Configure(configuration)
                .Mappings(m =>
                    m.FluentMappings
                        .Add<AdministrationUnitMap>()
                        .Add<EntranceMap>()
                        .Add<SubUnitMap>()
                        .Add<AddressMap>()
                        .Add<PostalCodeInfoMap>()
                        .Add<AdministrationUnitsFeatureMap>()
                        .Add<AdministrationUnitPropertyMap>()
                )
                .ExposeConfiguration(BuildSchema)
                .BuildConfiguration()
                .BuildSessionFactory();
        }

        private static ISession FromFactory(ISessionFactory factory, bool readOnly)
        {
            return factory
                .WithOptions()
                .ConnectionReleaseMode(ConnectionReleaseMode.OnClose)
                .NoInterceptor()
                .FlushMode(readOnly ? FlushMode.Manual : FlushMode.Auto)
                .OpenSession();
        }

        private static ISession FetchSession(string key, bool readOnly)
        {
            if (string.IsNullOrEmpty(key)) {
                Initialize();
                return FromFactory(_sessionFactory, readOnly);
            }

            var factory = SessionFactories.fetch(key);
            return FromFactory(factory, readOnly);
        }

        private static Unit DropFactory(ISessionFactory factory)
        {
            factory.Dispose();
            return null;
        }
        
        private static void BuildSchema(NHibernate.Cfg.Configuration config)
        {
            new SchemaUpdate(config).Execute(false, true);
        }
        
        public static void Initialize(FluentConfiguration configuration)
        {
            _sessionFactory?.Dispose();

            _sessionFactory = configuration
                .Mappings(m =>
                    m.FluentMappings
                        .Add<AdministrationUnitMap>()
                        .Add<EntranceMap>()
                        .Add<SubUnitMap>()
                        .Add<AddressMap>()
                        .Add<PostalCodeInfoMap>()
                        .Add<AdministrationUnitsFeatureMap>()
                        .Add<AdministrationUnitPropertyMap>()
                )
                .BuildConfiguration()
                .BuildSessionFactory();
        }

        public static void Shutdown()
        {
            _sessionFactory?.Dispose();
            _sessionFactory = null;
        }
        
        public static ISession Open()
        {
            return FetchSession(null, false);
        }
        
        public static ISession OpenReadOnly()
        {
            return FetchSession(null, true);
        }
        
        public static ISession Open(string key)
        {
            return FetchSession(key, false);
        }
        
        public static ISession OpenReadOnly(string key)
        {
            return FetchSession(key, true);
        }
    }
}