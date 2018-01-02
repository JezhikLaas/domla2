namespace D2.Authentication

module PersistedGrantData =

    open IdentityServer4.Models
    open Npgsql
    open System
    open System.Data.Common

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
                
            command.Parameters << ("subject_id", subjectId) |> ignore
                
            use! reader = command.ExecuteReaderAsync () |> Async.AwaitTask

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
                
            command.Parameters << ("key", key) |> ignore
                
            use! reader = command.ExecuteReaderAsync () |> Async.AwaitTask

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
                
            command.Parameters << ("subject_id", subjectId)
                               << ("client_id", clientId) |> ignore

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
                
            command.Parameters << ("subject_id", subjectId)
                               << ("client_id", clientId)
                               << ("type", grantType) |> ignore

            let! result = command.ExecuteNonQueryAsync () |> Async.AwaitTask
            ()
        }
        
    let removeOutdated (options : ConnectionOptions) () =
        async {
            use connection = authentication options
            use command = connection.CreateCommand ()
            command.CommandText <- """DELETE FROM
                                          persisted_grants
                                      WHERE
                                          expiration IS NOT NULL
                                          AND
                                          expiration < 'now'"""
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
                
            command.Parameters << ("key", key) |> ignore

            let! result = command.ExecuteNonQueryAsync () |> Async.AwaitTask
            ()
        }
        
    let store (options : ConnectionOptions) (grant : PersistedGrant) =
        async {
            use connection = authentication options
            use command = connection.CreateCommand ()
            command.CommandText <- """INSERT INTO
                                          persisted_grants (
                                              key,
                                              type,
                                              subject_id,
                                              client_id,
                                              creation_time,
                                              expiration,
                                              data
                                          )
                                          VALUES (
                                              :key,
                                              :type,
                                              :subject_id,
                                              :client_id,
                                              :creation_time,
                                              :expiration,
                                              :data
                                          )
                                          ON CONFLICT (key) DO UPDATE SET
                                              type = EXCLUDED.type,
                                              subject_id = EXCLUDED.subject_id,
                                              client_id = EXCLUDED.client_id,
                                              creation_time = EXCLUDED.creation_time,
                                              expiration = EXCLUDED.expiration,
                                              data = EXCLUDED.data"""
                
            command.Parameters << ("key", grant.Key)
                               << ("type", grant.Type)
                               << ("subject_id", grant.SubjectId)
                               << ("client_id", grant.ClientId)
                               << ("creation_time", grant.CreationTime)
                               << ("data", grant.Data) |> ignore
            if grant.Expiration.HasValue then
                command.Parameters << ("expiration", grant.Expiration) |> ignore

            let! result = command.ExecuteNonQueryAsync () |> Async.AwaitTask
            ()
        }

    let access (options : ConnectionOptions) =
        {
            getAll = getAll options;
            get = get options;
            removeAll = removeAll options;
            removeAllType = removeAllType options;
            remove = remove options;
            removeOutdated = removeOutdated options;
            store = store options;
        }

