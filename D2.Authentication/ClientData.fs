﻿namespace D2.Authentication

module ClientData =

    open D2.Common
    open IdentityServer4.Models
    open Json
    open Npgsql
    open System.Data.Common

    let private json = Json.Converter Json.jsonOptions

    let private findClientById (options : ConnectionOptions) (clientId : string) =
        async {
            use connection = authentication options
            use command = connection.CreateCommand ()
            command.CommandText <- """SELECT
                                          data
                                      FROM
                                          clients
                                      WHERE
                                          id = :id"""
                
            command.Parameters << ("id", StringField clientId) |> ignore
                
            use! reader = command.ExecuteReaderAsync () |> Async.AwaitTask

            match reader.Read () with
            | true  -> return Some (json.deserialize<Client> (reader.GetString 0))
            | false -> return None
        }

    let access (options : ConnectionOptions) =
        {
            findClientById = findClientById options;
        }
    
