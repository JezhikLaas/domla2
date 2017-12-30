namespace D2.Authentication

module Storage =

    open BCrypt.Net
    open IdentityServer4
    open IdentityServer4.Models
    open Newtonsoft.Json
    open Npgsql
    open System
    open System.Data.Common
    open System.Security.Claims

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
                
                command.Parameters.AddWithValue("key", key) |> ignore
                
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
                removeOutdated = removeOutdated options;
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
                
                use! reader = command.ExecuteReaderAsync () |> Async.AwaitTask

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
                
                use! reader = command.ExecuteReaderAsync () |> Async.AwaitTask

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
                
                use! reader = command.ExecuteReaderAsync () |> Async.AwaitTask

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
                
                let identityResources () = 
                    use commandIdentities = connection.CreateCommand ()
                    commandIdentities.CommandText <- """SELECT
                                                            data
                                                        FROM
                                                            identity_resources"""
                
                    use readerIdentities = commandIdentities.ExecuteReader ()
                
                    seq {
                        while readerIdentities.Read () do
                            yield JsonConvert.DeserializeObject<IdentityResource>(readerIdentities.GetString(0))
                    }
                    |> Seq.toList
                
                let apiResources () = 
                    use commandApis = connection.CreateCommand ()
                    commandApis.CommandText <- """SELECT
                                                        data
                                                    FROM
                                                        api_resources"""
                    
                    use readerApis = commandApis.ExecuteReader ()

                    seq {
                        while readerApis.Read () do
                            yield JsonConvert.DeserializeObject<ApiResource>(readerApis.GetString(0))
                    }
                    |> Seq.toList
                
                return new Resources (
                    identityResources (),
                    apiResources ()
                )
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
                
                use! reader = command.ExecuteReaderAsync () |> Async.AwaitTask

                match reader.Read () with
                | true  -> return Some (Newtonsoft.Json.JsonConvert.DeserializeObject<Client>(reader.GetString(0)))
                | false -> return None
            }

        let access (options : ConnectionOptions) =
            {
                findClientById = findClientById options;
            }
    
    module UserData = 
        
        let findUser (options : ConnectionOptions) (name : string) (password : string) =
            async {
                use connection = authentication options
                use command = connection.CreateCommand()
    
                command.CommandText <- """SELECT
                                              id,
                                              login,
                                              first_name,
                                              last_name,
                                              email,
                                              password,
                                              title,
                                              salutation,
                                              claims,
                                              logged_in
                                          FROM
                                              users
                                          WHERE
                                              login = :login"""
                command.Parameters.AddWithValue("login", name) |> ignore
    
                use! reader = command.ExecuteReaderAsync() |> Async.AwaitTask
                match reader.Read() with
                | true  -> let storedPassword = reader.GetString(5)
                           match BCrypt.Verify(password, storedPassword) with
                           | true  -> return Some (UserI.fromReader reader)
                           | false -> return None
                | false -> return None
            }
    
        let fetchUser (options : ConnectionOptions) (id : string) =
            async {
                use connection = authentication options
                use command = connection.CreateCommand()
    
                command.CommandText <- "SELECT id, login, first_name, last_name, email, title, salutation, claims, logged_in FROM users WHERE id = :id"
                command.Parameters.AddWithValue("id", new Guid(id)) |> ignore
    
                use! reader = command.ExecuteReaderAsync() |> Async.AwaitTask
                match reader.Read() with
                | true  -> return Some (UserI.fromReader reader)
                | false -> return None
            }
        
        let updateActive (options : ConnectionOptions) (id : string) (state : bool) =
            async {
                use connection = authentication options
                use command = connection.CreateCommand()
    
                command.CommandText <- sprintf "UPDATE users SET logged_in = %s FROM WHERE id = :id" (if state then "LOCALTIMESTAMP" else "NULL")
                command.Parameters.AddWithValue("id", new Guid(id)) |> ignore
    
                let! result = command.ExecuteNonQueryAsync() |> Async.AwaitTask
                match result with
                | 1 -> return ()
                | _ -> failwith "failed to update login state"
            }

        let access (options : ConnectionOptions) =
            {
                findUser = findUser options;
                fetchUser = fetchUser options;
                updateActive = updateActive options;
            }
        
    module SetupData =

        let initialize (options : ConnectionOptions) () =
            async {
                use connection = authentication options

                let isDatabaseFilled () =
                    use command = connection.CreateCommand ()
                    command.CommandText <- """SELECT
                                                  CASE WHEN EXISTS (SELECT 1 FROM clients)
                                                      THEN 1
                                                      ELSE 0
                                                  END"""
                
                    match command.ExecuteScalar () :?> int32 with
                    | 1 -> true
                    | _ -> false
                
                let insertClients () =
                    use command = connection.CreateCommand ()
                    let silicon = Client (
                                      ClientId = "service",
                                      AllowedGrantTypes = GrantTypes.ClientCredentials,
                                      ClientSecrets = [| Secret ("{E9B7C075-8704-4BC4-BB17-B45AFC8CCB5C}".Sha256 ()) |],
                                      AllowedScopes = [| "api" |]
                                  )
                    let interactive = Client (
                                          ClientId = "interactive",
                                          ClientName = "Interactive user",
                                          AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                                          ClientSecrets = [| Secret ("{513501CB-6B8F-4D22-8D1C-6A32EA6C589B}".Sha256 ()) |],
                                          RedirectUris = [| "http://localhost:5002/signin-oidc" |],
                                          PostLogoutRedirectUris = [| "http://localhost:5002/signout-callback-oidc" |],
                                          RequireConsent = false,
                                          AllowOfflineAccess = true,
                                          AllowedScopes =
                                              [|
                                                  IdentityServerConstants.StandardScopes.OpenId;
                                                  IdentityServerConstants.StandardScopes.Profile;
                                                  "role.profile";
                                                  "api"
                                              |]
                                      )
                    
                    command.CommandText <- """INSERT INTO
                                                  clients (id, data)
                                              VALUES
                                                  (:id, :data)"""
                    
                    for client in [| silicon; interactive |] do
                        command.Parameters.Clear ()
                        command.Parameters.AddWithValue ("id", client.ClientId) |> ignore
                        command.Parameters.AddWithValue (
                            "data",
                            NpgsqlTypes.NpgsqlDbType.Jsonb,
                            JsonConvert.SerializeObject (client)
                        ) |> ignore
                        command.ExecuteNonQuery () |> ignore

                let insertIdentities () =
                    use command = connection.CreateCommand ()
                    command.CommandText <- """INSERT INTO
                                                  identity_resources (name, data)
                                              VALUES
                                                  (:name, :data)"""
                    
                    let resources = 
                        [|
                            IdentityResource(
                                name = "role.profile",
                                displayName = "Role profile",
                                claimTypes = [| "role" |]);
                            IdentityResources.OpenId() :> IdentityResource;
                            IdentityResources.Profile() :> IdentityResource;
                        |]
                    
                    for resource in resources do
                        command.Parameters.Clear ()
                        command.Parameters.AddWithValue ("name", resource.Name) |> ignore
                        command.Parameters.AddWithValue (
                            "data",
                            NpgsqlTypes.NpgsqlDbType.Jsonb,
                            JsonConvert.SerializeObject (resource)
                        ) |> ignore
                        command.ExecuteNonQuery () |> ignore
                
                let insertApis () =
                    use command = connection.CreateCommand ()
                    command.CommandText <- """INSERT INTO
                                                  api_resources (name, data)
                                              VALUES
                                                  (:name, :data)"""
                    
                    let resources = 
                        [|
                            ApiResource ("api", "REST Api")
                        |]
                    
                    for resource in resources do
                        command.Parameters.Clear ()
                        command.Parameters.AddWithValue ("name", resource.Name) |> ignore
                        command.Parameters.AddWithValue (
                            "data",
                            NpgsqlTypes.NpgsqlDbType.Jsonb,
                            JsonConvert.SerializeObject (resource)
                        ) |> ignore
                        command.ExecuteNonQuery () |> ignore
                
                let insertAdmin () =
                    let salt = BCrypt.GenerateSalt()
                    let password = BCrypt.HashPassword("secret", salt)

                    use insert = connection.CreateCommand()
                    insert.CommandText <- "INSERT INTO users (id, login, password, last_name, email, claims) VALUES (:id, :login, :password, :last_name, :email, :claims)"
                    insert.Parameters.AddWithValue("id", NpgsqlTypes.NpgsqlDbType.Uuid, Guid.NewGuid()) |> ignore
                    insert.Parameters.AddWithValue("login", NpgsqlTypes.NpgsqlDbType.Varchar, "admin") |> ignore
                    insert.Parameters.AddWithValue("password", NpgsqlTypes.NpgsqlDbType.Varchar, password) |> ignore
                    insert.Parameters.AddWithValue("last_name", NpgsqlTypes.NpgsqlDbType.Varchar, "<unknown>") |> ignore
                    insert.Parameters.AddWithValue("email", NpgsqlTypes.NpgsqlDbType.Varchar, "<unknown>") |> ignore
                    insert.Parameters.AddWithValue("claims", NpgsqlTypes.NpgsqlDbType.Jsonb, JsonConvert.SerializeObject([| new Claim("role", "admin") |])) |> ignore
                    insert.ExecuteNonQuery() |> ignore
                
                match isDatabaseFilled () with
                | true  -> ()
                | false -> insertClients ()
                           insertIdentities ()
                           insertApis ()
                           insertAdmin ()
            }

        let access (options : ConnectionOptions) =
            {
                initialize = initialize options;
            }

    let storages = {
        setupStorage = SetupData.access;
        persistedGrantStorage = PersistedGrantData.access;
        resourceStorage = ResourceData.access;
        clientStorage = ClientData.access;
        userStorage = UserData.access;
    }
