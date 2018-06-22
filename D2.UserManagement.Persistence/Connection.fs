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
    open FluentNHibernate.Cfg
    open NHibernate
    open NHibernate.Cfg
    open NHibernate.Tool.hbm2ddl
    open System
    open System.Reflection

    let configurationFromFile = ServiceConfiguration.connectionInfo
    
    let private connectionString =
        fun () -> 
            let builder = new NpgsqlConnectionStringBuilder()
            builder.ApplicationName <- configurationFromFile.Identifier
            builder.Database <- configurationFromFile.Name
            builder.Host <- configurationFromFile.Host
            builder.Password <- configurationFromFile.Password
            builder.Username <- configurationFromFile.User
            builder.Port <- configurationFromFile.Port
            
            builder.ConnectionString
    
    let private connectionProperties = Map.empty
                                        .Add("connection.connection_string", connectionString ())
                                        .Add("connection.driver_class", "Beginor.NHibernate.NpgSql.NpgSqlDriver,NHibernate.NpgSql")
                                        .Add("dialect", "NHibernate.Dialect.PostgreSQL83Dialect")
                                        .Add("use_proxy_validator", "false")
    
    let private configuration = Configuration().SetProperties(connectionProperties)

    let mutable (private sessionFactory : ISessionFactory) = null  
    
    let initialize (configuration : FluentConfiguration) =
        if sessionFactory <> null then sessionFactory.Dispose()
        sessionFactory <- configuration
                              .Mappings(fun m -> m.FluentMappings
                                                  .Add<UserMap>()
                                                  .Add<UserRegistrationMap>()
                                                  |> ignore)
                              .BuildConfiguration()
                              .BuildSessionFactory()

    let shutdown () =
        if sessionFactory <> null then
            sessionFactory.Dispose()
            sessionFactory <- null

    let private defaultInitialize () =
        if sessionFactory = null then
            Fluently.Configure(configuration)
                    .Mappings(fun m -> m.FluentMappings
                                        .Add<UserCreateMap>()
                                        .Add<UserRegistrationCreateMap>()
                                        |> ignore)
                    .ExposeConfiguration(fun config -> (SchemaUpdate(config)).Execute(false, true))
                    .BuildConfiguration()
                    |> ignore
            initialize (Fluently.Configure(configuration))

    let session () =
        defaultInitialize ()
        sessionFactory
            .WithOptions()
            .ConnectionReleaseMode(ConnectionReleaseMode.OnClose)
            .NoInterceptor()
            .FlushMode(FlushMode.Auto)
            .OpenSession()
    
    let readOnlySession () =
        defaultInitialize ()
        sessionFactory
            .WithOptions()
            .ConnectionReleaseMode(ConnectionReleaseMode.OnClose)
            .NoInterceptor()
            .FlushMode(FlushMode.Manual)
            .OpenSession()
