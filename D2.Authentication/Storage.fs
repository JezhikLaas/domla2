namespace D2.Authentication

module Storage =

    open IdentityServer4.Models
    open Newtonsoft.Json
    open Npgsql
    open System
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
                                                  SET type = EXCLUDED.type,
                                                  SET subject_id = EXCLUDED.subject_id,
                                                  SET client_id = EXCLUDED.client_id,
                                                  SET creation_time = EXCLUDED.creation_time,
                                                  SET expiration = EXCLUDED.expiration,
                                                  SET data = EXCLUDED.data"""
                
                command.Parameters.AddWithValue("key", grant.Key) |> ignore
                command.Parameters.AddWithValue("type", grant.Type) |> ignore
                command.Parameters.AddWithValue("subject_id", grant.SubjectId) |> ignore
                command.Parameters.AddWithValue("client_id", grant.ClientId) |> ignore
                command.Parameters.AddWithValue("creation_time", grant.CreationTime) |> ignore
                if grant.Expiration.HasValue then
                    command.Parameters.AddWithValue("expiration", grant.Expiration) |> ignore
                command.Parameters.AddWithValue("data", grant.Data) |> ignore

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
                store = store options;
            }

    module ResourceData =

        let findApiResource (options : ConnectionOptions) (name : string) =
            async {
                use connection = authentication options
                use command = connection.CreateCommand ()
                command.CommandText <- """SELECT
                                              data
                                          FROM
                                              api_resources
                                          WHERE
                                              name = :name"""
                
                command.Parameters.AddWithValue("name", name) |> ignore
                
                let! reader = command.ExecuteReaderAsync () |> Async.AwaitTask
                match reader.Read () with
                | true  -> return Some (JsonConvert.DeserializeObject<ApiResource>(reader.GetString(0)))
                | false -> return None
            }

        let findApiResourcesByScope (options : ConnectionOptions) (names : string seq) =
            async {
                use connection = authentication options
                use command = connection.CreateCommand ()
                command.CommandText <- sprintf
                                        """SELECT
                                               data
                                           FROM
                                               api_resources
                                           WHERE
                                               data -> Scopes ->> Name IN (%s)"""
                                        (String.Join (",", names))
                
                let! reader = command.ExecuteReaderAsync () |> Async.AwaitTask

                return seq {
                    while reader.Read () do
                        yield JsonConvert.DeserializeObject<ApiResource>(reader.GetString(0))
                }
                |> Seq.toList
                |> List.toSeq
            }

        let findIdentityResourcesByScope (options : ConnectionOptions) (names : string seq) =
            async {
                use connection = authentication options
                use command = connection.CreateCommand ()
                command.CommandText <- sprintf
                                        """SELECT
                                               data
                                           FROM
                                               identity_resources
                                           WHERE
                                               name IN (%s)"""
                                        (String.Join (",", names))
                
                let! reader = command.ExecuteReaderAsync () |> Async.AwaitTask

                return seq {
                    while reader.Read () do
                        yield JsonConvert.DeserializeObject<IdentityResource>(reader.GetString(0))
                }
                |> Seq.toList
                |> List.toSeq
            }

        let getAllResources (options : ConnectionOptions) () =
            async {
                use connection = authentication options
                
                use commandIdentities = connection.CreateCommand ()
                commandIdentities.CommandText <- """SELECT
                                                        data
                                                    FROM
                                                        identity_resources"""
                
                use commandApis = connection.CreateCommand ()
                commandApis.CommandText <- """SELECT
                                                  data
                                              FROM
                                                  api_resources"""
                
                let! readerIdentities = commandIdentities.ExecuteReaderAsync () |> Async.AwaitTask
                let! readerApis = commandApis.ExecuteReaderAsync () |> Async.AwaitTask

                let identityResources = seq {
                    while readerIdentities.Read () do
                        yield JsonConvert.DeserializeObject<IdentityResource>(readerIdentities.GetString(0))
                }
                let apiResources = seq {
                    while readerApis.Read () do
                        yield JsonConvert.DeserializeObject<ApiResource>(readerIdentities.GetString(0))
                }
                
                return new Resources (identityResources, apiResources)
            }

        let access (options : ConnectionOptions) =
            {
                findApiResource = findApiResource options;
                findApiResourcesByScope = findApiResourcesByScope options;
                findIdentityResourcesByScope = findIdentityResourcesByScope options;
                getAllResources = getAllResources options;
            }

    module ClientData =

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
                
                command.Parameters.AddWithValue("id", id) |> ignore
                
                let! reader = command.ExecuteReaderAsync () |> Async.AwaitTask
                match reader.Read () with
                | true  -> return Some (Newtonsoft.Json.JsonConvert.DeserializeObject<Client>(reader.GetString(0)))
                | false -> return None
            }

        let access (options : ConnectionOptions) =
            {
                findClientById = findClientById options;
            }

    let storages = {
        persistedGrantStorage = PersistedGrantData.access;
        resourceStorage = ResourceData.access;
        clientStorage = ClientData.access
    }
