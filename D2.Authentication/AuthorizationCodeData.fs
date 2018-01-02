namespace D2.Authentication

module AuthorizationCodeData =
    
    open IdentityServer4.Models
    open Newtonsoft.Json
    open Npgsql
    open System
    open System.Data.Common

    let storeAuthorizationCode (options : ConnectionOptions) (value : AuthorizationCode) =
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
            command.Parameters << ("id", key)
                              <<< ("data", NpgsqlTypes.NpgsqlDbType.Jsonb, JsonConvert.SerializeObject(value, Json.jsonOptions)) |> ignore

            let! _ = command.ExecuteNonQueryAsync() |> Async.AwaitTask
            return key
        }

    let getAuthorizationCode (options : ConnectionOptions) (key : string) =
        async {
            use connection = authentication options
            use command = connection.CreateCommand()
        
            command.CommandText <- "SELECT data FROM authorization_codes WHERE id = :id"
            command.Parameters << ("id", key) |> ignore
        
            use! reader = command.ExecuteReaderAsync() |> Async.AwaitTask
            match reader.Read() with
            | true  -> let result = JsonConvert.DeserializeObject<AuthorizationCode>(reader.GetString(0), Json.jsonOptions)
                       return Some result
            | false -> return None
        }

    let removeAuthorizationCode (options : ConnectionOptions) (key : string) =
        async {
            use connection = authentication options
            use command = connection.CreateCommand()
        
            command.CommandText <- "DELETE FROM authorization_codes WHERE id = :id"
            command.Parameters << ("id", key) |> ignore

            let! _ = command.ExecuteNonQueryAsync() |> Async.AwaitTask
            ()
        }

    let access (options : ConnectionOptions) =
        {
            storeAuthorizationCode = storeAuthorizationCode options;
            getAuthorizationCode = getAuthorizationCode options;
            removeAuthorizationCode = removeAuthorizationCode options;
        }


