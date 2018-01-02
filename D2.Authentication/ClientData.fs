namespace D2.Authentication

module ClientData =

    open IdentityServer4.Models
    open Newtonsoft.Json
    open Npgsql
    open System.Data.Common

    let findClientById (options : ConnectionOptions) (clientId : string) =
        async {
            use connection = authentication options
            use command = connection.CreateCommand ()
            command.CommandText <- """SELECT
                                          data
                                      FROM
                                          clients
                                      WHERE
                                          id = :id"""
                
            command.Parameters << ("id", id) |> ignore
                
            use! reader = command.ExecuteReaderAsync () |> Async.AwaitTask

            match reader.Read () with
            | true  -> return Some (Newtonsoft.Json.JsonConvert.DeserializeObject<Client>(reader.GetString(0)))
            | false -> return None
        }

    let access (options : ConnectionOptions) =
        {
            findClientById = findClientById options;
        }
    
