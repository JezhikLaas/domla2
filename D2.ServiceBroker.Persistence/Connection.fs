namespace D2.ServiceBroker.Persistence

open Npgsql

type Postgres_Type = {
    Database : string
    Port : int32
    Host : string
    User : string
    Password : string 
}

module Connection = 

    let mutable connectionInfo = {
        Database = "D2.ServiceBroker";
        Port = 5432;
        Host = "localhost";
        User = "d2broker";
        Password = "d2broker" 
    }
    
    let mutable connectionProvider =
        fun () -> 
            let builder = new NpgsqlConnectionStringBuilder()
            builder.ApplicationName <- "Domla / 2 Service Broker"
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
