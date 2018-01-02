namespace D2.Authentication

[<AutoOpen>]
module StoreAuthentication =

    open Npgsql
    open System.Data.Common

    let authentication (options : ConnectionOptions) =
        let builder = new NpgsqlConnectionStringBuilder()
        builder.ApplicationName <- "Domla/2 Authentication"
        builder.Database <- options.Database
        builder.Host <- options.Host
        builder.Password <- options.Password
        builder.Port <- options.Port
        builder.Username <- options.User

        let result = new NpgsqlConnection(builder.ConnectionString)
        result.Open()
        result

