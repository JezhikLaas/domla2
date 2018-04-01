namespace D2.UserManagement.Persistence

open D2.Common
open Npgsql

type Postgres_Type = {
    Database : string
    Port : int32
    Host : string
    User : string
    Password : string 
}

module Connection = 
    open NHibernate
    open NHibernate.Cfg
    open System.Reflection

    let configurationFromFile = ServiceConfiguration.connectionInfo

    let mutable connectionInfo = {
        Database = configurationFromFile.Name;
        Port = configurationFromFile.Port;
        Host = configurationFromFile.Host;
        User = configurationFromFile.User;
        Password = configurationFromFile.Password 
    }
    
    let mutable connectionProvider =
        fun () -> 
            let builder = new NpgsqlConnectionStringBuilder()
            builder.ApplicationName <- configurationFromFile.Identifier
            builder.Database <- connectionInfo.Database
            builder.Host <- connectionInfo.Host
            builder.Password <- connectionInfo.Password
            builder.Username <- connectionInfo.User
            builder.Port <- connectionInfo.Port
    
            new NpgsqlConnection(builder.ConnectionString)

    let client () =
        let result = connectionProvider ()
        result.Open()
        result
    
    let mutable connectionString =
        fun () -> 
            let builder = new NpgsqlConnectionStringBuilder()
            builder.ApplicationName <- configurationFromFile.Identifier
            builder.Database <- connectionInfo.Database
            builder.Host <- connectionInfo.Host
            builder.Password <- connectionInfo.Password
            builder.Username <- connectionInfo.User
            builder.Port <- connectionInfo.Port
            
            builder.ConnectionString
    
    let connectionProperties = Map.empty
                                .Add("connection.connection_string", connectionString ())
                                .Add("connection.driver_class", "Beginor.NHibernate.NpgSql.NpgSqlDriver,NHibernate.NpgSql")
                                .Add("dialect", "NHibernate.Dialect.PostgreSQL83Dialect")
                                .Add("use_proxy_validator", "false")
    let sessionFactory = Configuration()
                            .SetProperties(connectionProperties)
                            .AddAssembly(Assembly.GetExecutingAssembly())
                            .BuildSessionFactory()

    let session () =
        sessionFactory
            .WithOptions()
            .ConnectionReleaseMode(ConnectionReleaseMode.OnClose)
            .NoInterceptor()
            .FlushMode(FlushMode.Auto)
            .OpenSession()
    
    let readOnlySession () =
        sessionFactory
            .WithOptions()
            .ConnectionReleaseMode(ConnectionReleaseMode.OnClose)
            .NoInterceptor()
            .FlushMode(FlushMode.Manual)
            .OpenSession()
