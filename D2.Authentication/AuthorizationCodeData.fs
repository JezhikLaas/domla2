namespace D2.Authentication

module AuthorizationCodeData =
    
    open D2.Common
    open IdentityServer4.Models
    open Json
    open Npgsql
    open System
    open System.Data.Common

    let private json = Json.Converter Json.jsonOptions

    let private storeAuthorizationCode (options : ConnectionOptions) (value : AuthorizationCode) =
        async {
            use connection = authentication options
            use command = connection.CreateCommand()
            let key = Guid.NewGuid().ToString("N")

            command.CommandText <- """INSERT INTO
                                          authorization_codes(id, data)
                                      VALUES
                                          (:id, :data)
                                      ON CONFLICT (id)
                                      DO UPDATE SET
                                          data = EXCLUDED.data"""
            command.Parameters << ("id", StringField key)
                               << ("data", json.serialize value) |> ignore

            let! _ = command.ExecuteNonQueryAsync() |> Async.AwaitTask
            return key
        }

    let private getAuthorizationCode (options : ConnectionOptions) (key : string) =
        async {
            use connection = authentication options
            use command = connection.CreateCommand()
        
            command.CommandText <- "SELECT data FROM authorization_codes WHERE id = :id"
            command.Parameters << ("id", StringField key) |> ignore
        
            use! reader = command.ExecuteReaderAsync() |> Async.AwaitTask
            match reader.Read() with
            | true  -> let result = json.deserialize<AuthorizationCode> (reader.GetString 0)
                       return Some result
            | false -> return None
        }

    let private removeAuthorizationCode (options : ConnectionOptions) (key : string) =
        async {
            use connection = authentication options
            use command = connection.CreateCommand()
        
            command.CommandText <- "DELETE FROM authorization_codes WHERE id = :id"
            command.Parameters << ("id", StringField key) |> ignore

            let! _ = command.ExecuteNonQueryAsync() |> Async.AwaitTask
            ()
        }

    let access (options : ConnectionOptions) =
        {
            storeAuthorizationCode = storeAuthorizationCode options;
            getAuthorizationCode = getAuthorizationCode options;
            removeAuthorizationCode = removeAuthorizationCode options;
        }


