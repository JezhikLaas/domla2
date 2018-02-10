namespace D2.ServiceBroker.Persistence

open Npgsql
open D2.Common

type Postgres_Type = {
    Database : string
    Port : int32
    Host : string
    User : string
    Password : string 
}

module Connection = 

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
