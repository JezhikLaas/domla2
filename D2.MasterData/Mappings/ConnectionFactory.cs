using System;
using System.Collections.Generic;
using D2.Common;
using D2.Service.IoC;
using FluentNHibernate.Cfg;
using Microsoft.Extensions.Logging;
using Microsoft.FSharp.Core;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Npgsql;

namespace D2.MasterData.Mappings
{
    [Singleton]
    public class ConnectionFactory : IConnectionFactory
    {
        private ISessionFactory _sessionFactory;
        private readonly Cache<ISessionFactory, string> _sessionFactories;
        private readonly object _syncRoot;
        private readonly ILogger<ConnectionFactory> _logger;
        
        public ConnectionFactory(ILogger<ConnectionFactory> logger)
        {
            _syncRoot = new object();
            _logger = logger;
            Func<string, ISessionFactory> createFactory = CreateFactory;
            Func<string, ISessionFactory, Unit> dropFactory = DropFactory;
            
            var options = new CacheOptions<ISessionFactory, string>(
                FSharpFuncUtil<string, ISessionFactory>.ToFSharpFunc(createFactory),
                FSharpFuncUtil<(string, ISessionFactory), Unit>.ToFSharpFunc(dropFactory),
                600,
                10
                );
            _sessionFactories = new Cache<ISessionFactory, string>(options);
        }

        public void Initialize()
        {
            lock (_syncRoot)
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

        private ISessionFactory CreateFactory(string key)
        {
            _logger.LogInformation($"Creating factory for {key}");
            
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
            
            var result = Fluently.Configure(configuration)
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
            
            _logger.LogTrace($"Created factory for {key}");

            return result;
        }

        private ISession FromFactory(ISessionFactory factory, bool readOnly)
        {
            return factory
                .WithOptions()
                .ConnectionReleaseMode(ConnectionReleaseMode.OnClose)
                .NoInterceptor()
                .FlushMode(readOnly ? FlushMode.Manual : FlushMode.Auto)
                .OpenSession();
        }

        private ISession FetchSession(string key, bool readOnly)
        {
            if (string.IsNullOrEmpty(key)) {
                Initialize();
                return FromFactory(_sessionFactory, readOnly);
            }

            var factory = _sessionFactories.fetch(key);
            return FromFactory(factory, readOnly);
        }

        private Unit DropFactory(string key, ISessionFactory factory)
        {
            _logger.LogDebug($"Dropping factory for {key}");
            factory.Dispose();
            return null;
        }
        
        private void BuildSchema(NHibernate.Cfg.Configuration config)
        {
            new SchemaUpdate(config).Execute(false, true);
        }
        
        public void Initialize(FluentConfiguration configuration)
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

        public void Shutdown()
        {
            _sessionFactory?.Dispose();
            _sessionFactory = null;
        }
        
        public ISession Open()
        {
            return FetchSession(null, false);
        }
        
        public ISession OpenReadOnly()
        {
            return FetchSession(null, true);
        }
        
        public ISession Open(string key)
        {
            return FetchSession(key, false);
        }
        
        public ISession OpenReadOnly(string key)
        {
            return FetchSession(key, true);
        }
    }
}