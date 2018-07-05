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
    
    let private specificConnectionString (db : string) (user : string) (pwd : string) =
        fun () -> 
            let builder = new NpgsqlConnectionStringBuilder()
            builder.ApplicationName <- configurationFromFile.Identifier
            builder.Database <- db
            builder.Host <- configurationFromFile.Host
            builder.Password <- pwd
            builder.Username <- user
            builder.Port <- configurationFromFile.Port
            
            builder.ConnectionString

    let fetchAdminRole () =
         configurationFromFile.User

    let connection () =
        let result = new NpgsqlConnection (connectionString ())
        result.Open ()
        
        result

    let connectSpecific (db : string) (user : string) (pwd : string) =
        let result = new NpgsqlConnection ((specificConnectionString db user pwd) ())
        result.Open ()
        
        result