namespace D2.UserManagement.Persistence

open D2.Common
open Npgsql

module AdminConnection =
    
    let configurationFromFile = ServiceConfiguration.adminConnectionInfo
    
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

    let connection () =
        let result = new NpgsqlConnection (connectionString ())
        result.Open ()
        
        result