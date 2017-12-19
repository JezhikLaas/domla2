namespace D2.Authentication

module Storage =

    open IdentityServer4.Models
    open Npgsql
    open System

    let authentication () =
        let builder = new NpgsqlConnectionStringBuilder()
        builder.ApplicationName <- "Domla / 2 Authentication"
        //builder.Database <- configuration.Postgres.Database
        //builder.Host <- configuration.Postgres.Host
        //builder.Password <- configuration.Postgres.Password
        //builder.Port <- configuration.Postgres.Port
        //builder.Username <- configuration.Postgres.User

        let result = new NpgsqlConnection(builder.ConnectionString)
        result.Open()
        result
    
    module PersistedGrantData =
        let getAll (subjectId : string) =
            async {
                use connection = authentication ()
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
                        yield PersistedGrant (
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
                }
                |> Seq.toList
                |> List.toSeq
            }

    let persistedGrantStore = {
            getAll = PersistedGrantData.getAll;
            get = fun _ -> async { return None };
            removeAll = fun _ _ -> async { () };
            removeAllType = fun _ _ _ -> async { () };
            remove = fun _ -> async { () };
            store = fun _ -> async { () };
        }

