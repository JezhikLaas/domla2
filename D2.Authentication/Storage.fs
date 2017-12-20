namespace D2.Authentication

module Storage =

    open IdentityServer4.Models
    open Npgsql
    open System
    open System.Data.Common

    let authentication (options : ConnectionOptions) =
        let builder = new NpgsqlConnectionStringBuilder()
        builder.ApplicationName <- "Domla / 2 Authentication"
        builder.Database <- options.Database
        builder.Host <- options.Host
        builder.Password <- options.Password
        builder.Port <- options.Port
        builder.Username <- options.User

        let result = new NpgsqlConnection(builder.ConnectionString)
        result.Open()
        result
    
    module PersistedGrantData =

        let fromReader (reader : DbDataReader) =
            PersistedGrant (
                Key = reader.GetString 0,
                Type = reader.GetString 1,
                SubjectId = reader.GetString 2,
                ClientId = reader.GetString 3,
                CreationTime = reader.GetDateTime 4,
                Expiration = (
                                if reader.IsDBNull 5 then
                                    Nullable<DateTime>()
                                else
                                    Nullable<DateTime>(reader.GetDateTime 5)
                            ),
                Data = reader.GetString 6
            )

        let getAll (options : ConnectionOptions) (subjectId : string) =
            async {
                use connection = authentication options
                use command = connection.CreateCommand ()
                command.CommandText <- """SELECT
                                              key,
                                              type,
                                              subject_id,
                                              client_id,
                                              creation_time,
                                              expiration,
                                              data
                                          FROM
                                              persisted_grants
                                          WHERE
                                              subject_id = :subject_id"""
                
                command.Parameters.AddWithValue("subject_id", subjectId) |> ignore
                
                let! reader = command.ExecuteReaderAsync () |> Async.AwaitTask

                return seq {
                    while reader.Read () do
                        yield fromReader reader
                }
                |> Seq.toList
                |> List.toSeq
            }
    
        let get (options : ConnectionOptions) (key : string) =
            async {
                use connection = authentication options
                use command = connection.CreateCommand ()
                command.CommandText <- """SELECT
                                              key,
                                              type,
                                              subject_id,
                                              client_id,
                                              creation_time,
                                              expiration,
                                              data
                                          FROM
                                              persisted_grants
                                          WHERE
                                              key = :key"""
                
                command.Parameters.AddWithValue("key", key) |> ignore
                
                let! reader = command.ExecuteReaderAsync () |> Async.AwaitTask
                match reader.Read () with
                | true  -> return Some (fromReader reader)
                | false -> return None
            }
        
        let removeAll (options : ConnectionOptions) (subjectId : string) (clientId : string) =
            async {
                use connection = authentication options
                use command = connection.CreateCommand ()
                command.CommandText <- """DELETE FROM
                                              persisted_grants
                                          WHERE
                                              subject_id = :subject_id
                                              AND
                                              client_id = :client_id"""
                
                command.Parameters.AddWithValue("subject_id", subjectId) |> ignore
                command.Parameters.AddWithValue("client_id", clientId) |> ignore

                let! result = command.ExecuteNonQueryAsync () |> Async.AwaitTask
                ()
            }
        
        let removeAllType (options : ConnectionOptions) (subjectId : string) (clientId : string) (grantType : string) =
            async {
                use connection = authentication options
                use command = connection.CreateCommand ()
                command.CommandText <- """DELETE FROM
                                              persisted_grants
                                          WHERE
                                              subject_id = :subject_id
                                              AND
                                              client_id = :client_id
                                              AND
                                              type = :type"""
                
                command.Parameters.AddWithValue("subject_id", subjectId) |> ignore
                command.Parameters.AddWithValue("client_id", clientId) |> ignore
                command.Parameters.AddWithValue("type", grantType) |> ignore

                let! result = command.ExecuteNonQueryAsync () |> Async.AwaitTask
                ()
            }
        
        let remove (options : ConnectionOptions) (key : string) =
            async {
                use connection = authentication options
                use command = connection.CreateCommand ()
                command.CommandText <- """DELETE FROM
                                              persisted_grants
                                          WHERE
                                              key = :key"""
                
                command.Parameters.AddWithValue("key", key) |> ignore

                let! result = command.ExecuteNonQueryAsync () |> Async.AwaitTask
                ()
            }

        let connection (options : ConnectionOptions) =
            let connectionOptions = options
            {
                getAll = getAll options;
                get = get options;
                removeAll = removeAll options;
                removeAllType = removeAllType options;
                remove = remove options;
                store = fun _ -> async { () };
            }

    let storages = {
        persistedGrantStorage = PersistedGrantData.connection
    }
