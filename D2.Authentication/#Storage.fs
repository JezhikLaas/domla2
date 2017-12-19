namespace D2.Authentication

open BCrypt.Net
open IdentityServer3.Core.Models
open Newtonsoft.Json
open Npgsql
open System
open System.Collections.Generic
open System.Linq
open System.Security.Claims

module Storage = 

    let private InsertDefaultClients (connection : NpgsqlConnection) =
        let checkDefaultClients =
            use command = connection.CreateCommand()
        
            command.CommandText <- "SELECT COUNT(*) FROM clients"
            (command.ExecuteScalar() :?> int64) > 0L

        match checkDefaultClients with
        | false -> let users = new Client()
                   users.ClientId <- "interactive"
                   users.ClientName <- "Interactive"
                   users.Enabled <- true
                   users.AccessTokenType <- AccessTokenType.Reference
                   users.Flow <- Flows.ResourceOwner
                   users.ClientSecrets <- new List<Secret>([new Secret("0A0C7C53-1A60-4D5D-AE4C-4163F72E467D".Sha256())])
                   users.AllowedScopes <- new List<string>(["admin"; "user"; "offline_access"; "openid"; "profile"; "all_claims"])
                   users.AlwaysSendClientClaims <- true
        
                   let services = new Client()
                   services.ClientId <- "silicon"
                   services.ClientName <- "Silicon-only Client"
                   services.Enabled <- true
                   services.AccessTokenType <- AccessTokenType.Reference
                   services.Flow <- Flows.ClientCredentials
                   services.ClientSecrets <- new List<Secret>([new Secret("1B0A7C32-1A60-4D5D-AE4C-4163F72E467D".Sha256())])
                   services.AllowedScopes <- new List<string>(["api"])
                   services.AlwaysSendClientClaims <- true
                   
                   use command = connection.CreateCommand()
                   command.CommandText <- "INSERT INTO clients (id, data) VALUES (:id, :data)"
                   command.Parameters.AddWithValue("id", users.ClientId) |> ignore
                   command.Parameters.AddWithValue("data", NpgsqlTypes.NpgsqlDbType.Jsonb, JsonConvert.SerializeObject(users)) |> ignore
                   command.ExecuteNonQuery() |> ignore

                   command.Parameters.Clear()
                   command.Parameters.AddWithValue("id", services.ClientId) |> ignore
                   command.Parameters.AddWithValue("data", NpgsqlTypes.NpgsqlDbType.Jsonb, JsonConvert.SerializeObject(services)) |> ignore
                   command.ExecuteNonQuery() |> ignore
        | true  -> ()

    let InsertDefaultScopes (connection : NpgsqlConnection) =
        let checkDefaultScopes =
            use command = connection.CreateCommand()
        
            command.CommandText <- "SELECT COUNT(*) FROM scopes"
            (command.ExecuteScalar() :?> int64) > 0L
        
        let defaultScopes = [
                StandardScopes.AddressAlwaysInclude;
                StandardScopes.AllClaims;
                StandardScopes.EmailAlwaysInclude;
                StandardScopes.OfflineAccess;
                StandardScopes.OpenId;
                StandardScopes.ProfileAlwaysInclude;
                StandardScopes.RolesAlwaysInclude;
                new Scope(Name = "user");
                new Scope(Name = "api");
            ]
        
        match checkDefaultScopes with
        | true  -> ()
        | false -> let command = connection.CreateCommand()
                   command.CommandText <- "INSERT INTO scopes (name, data) VALUES (:name, :data)"

                   for scope in defaultScopes do
                       command.Parameters.Clear()
                       command.Parameters.AddWithValue("name", NpgsqlTypes.NpgsqlDbType.Varchar, scope.Name) |> ignore
                       command.Parameters.AddWithValue("data", NpgsqlTypes.NpgsqlDbType.Jsonb, JsonConvert.SerializeObject(scope, Json.jsonOptions)) |> ignore
                       command.ExecuteNonQuery() |> ignore
    
    let InsertDefaultUsers (connection : NpgsqlConnection) =
        let checkDefaultUser =
            use command = connection.CreateCommand()
        
            command.CommandText <- "SELECT COUNT(*) FROM users"
            (command.ExecuteScalar() :?> int64) > 0L
        
        match checkDefaultUser with
        | false -> let salt = BCrypt.GenerateSalt()
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
        | true  -> ()

    let authentication () =
        let builder = new NpgsqlConnectionStringBuilder()
        builder.ApplicationName <- "Domla / 2 Authentication"
        builder.Database <- configuration.Postgres.Database
        builder.Host <- configuration.Postgres.Host
        builder.Password <- configuration.Postgres.Password
        builder.Port <- configuration.Postgres.Port
        builder.Username <- configuration.Postgres.User

        let result = new NpgsqlConnection(builder.ConnectionString)
        result.Open()
        InsertDefaultUsers (result)
        InsertDefaultScopes (result)
        InsertDefaultClients (result)
        result

    let findUser (name : string) (password : string) =
        async {
            use connection = authentication()
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
    
    let fetchUser (id : string) =
        async {
            use connection = authentication()
            use command = connection.CreateCommand()
    
            command.CommandText <- "SELECT id, login, first_name, last_name, email, title, salutation, claims, logged_in FROM users WHERE id = :id"
            command.Parameters.AddWithValue("id", new Guid(id)) |> ignore
    
            use! reader = command.ExecuteReaderAsync() |> Async.AwaitTask
            match reader.Read() with
            | true  -> return Some (UserI.fromReader reader)
            | false -> return None
        }
        
    let updateActive (id : string) (state : bool) =
        async {
            use connection = authentication()
            use command = connection.CreateCommand()
    
            command.CommandText <- sprintf "UPDATE users SET logged_in = %s FROM WHERE id = :id" (if state then "LOCALTIMESTAMP" else "NULL")
            command.Parameters.AddWithValue("id", new Guid(id)) |> ignore
    
            let! result = command.ExecuteNonQueryAsync() |> Async.AwaitTask
            match result with
            | 1 -> return ()
            | _ -> failwith "failed to update login state"
        }
        
    let loadAllScopes () =
        async {
            use connection = authentication()
            use command = connection.CreateCommand()
    
            command.CommandText <- "SELECT data FROM scopes"
            
            use! reader = command.ExecuteReaderAsync() |> Async.AwaitTask
            return seq {
                while reader.Read() do yield JsonConvert.DeserializeObject<Scope>(reader.GetString(0), Json.jsonOptions)
            }
        }

    let loadScopes (names : string seq) =
        async {
            use connection = authentication()
            use command = connection.CreateCommand()
    
            command.CommandText <- "SELECT data FROM scopes WHERE name IN (" + String.Join(", ", names) + ")"
            
            use! reader = command.ExecuteReaderAsync() |> Async.AwaitTask
            return seq {
                while reader.Read() do yield JsonConvert.DeserializeObject<Scope>(reader.GetString(0), Json.jsonOptions)
            }
        }

    let fetchClient (id : string) =
        async {
            use connection = authentication()
            use command = connection.CreateCommand()
            
            command.CommandText <- "SELECT data FROM clients WHERE id = :id"
            command.Parameters.AddWithValue("id", NpgsqlTypes.NpgsqlDbType.Varchar, id) |> ignore
        
            use! reader = command.ExecuteReaderAsync() |> Async.AwaitTask
            match reader.Read() with
            | true  -> return Some (JsonConvert.DeserializeObject<Client>(reader.GetString(0), Json.jsonOptions))
            | false -> return None
        }
    
    let storeAuthorizationCode (key : string) (value : AuthorizationCode) =
        async {
            use connection = authentication()
            use command = connection.CreateCommand()

            command.CommandText <- "INSERT INTO authorization_codes(id, data) VALUES (:id, :data) ON CONFLICT (id) DO UPDATE SET data = EXCLUDED.data"
            command.Parameters.AddWithValue("id", NpgsqlTypes.NpgsqlDbType.Varchar, key) |> ignore
            command.Parameters.AddWithValue("data", NpgsqlTypes.NpgsqlDbType.Jsonb, JsonConvert.SerializeObject(value, Json.jsonOptions)) |> ignore

            let! _ = command.ExecuteNonQueryAsync() |> Async.AwaitTask
            ()
        }

    let getAuthorizationCode (key : string) =
        async {
            use connection = authentication()
            use command = connection.CreateCommand()
        
            command.CommandText <- "SELECT data FROM authorization_codes WHERE id = :id"
            command.Parameters.AddWithValue("id", NpgsqlTypes.NpgsqlDbType.Varchar, key) |> ignore
        
            use! reader = command.ExecuteReaderAsync() |> Async.AwaitTask
            match reader.Read() with
            | true  -> let result = JsonConvert.DeserializeObject<AuthorizationCode>(reader.GetString(0), Json.jsonOptions)
                       return Some result
            | false -> return None
        }

    let deleteAuthorizationCode (key : string) =
        async {
            use connection = authentication()
            use command = connection.CreateCommand()
        
            command.CommandText <- "DELETE FROM authorization_codes WHERE id = :id"
            command.Parameters.AddWithValue("id", NpgsqlTypes.NpgsqlDbType.Varchar, key) |> ignore

            let! _ = command.ExecuteNonQueryAsync() |> Async.AwaitTask
            ()
        }

    let revokeAuthorizationCode (subject : string) (client : string) =
        async {
            use connection = authentication()
            use command = connection.CreateCommand()
        
            command.CommandText <- "DELETE FROM authorization_codes WHERE data->>ClientId = :client AND data->>SubjectId = :subject"
            command.Parameters.AddWithValue("client", NpgsqlTypes.NpgsqlDbType.Varchar, client) |> ignore
            command.Parameters.AddWithValue("subject", NpgsqlTypes.NpgsqlDbType.Varchar, subject) |> ignore

            let! _ = command.ExecuteNonQueryAsync() |> Async.AwaitTask
            return ()
        }
    
    let getAuthorizationCodes () =
        async {
            use connection = authentication()
            use command = connection.CreateCommand()
        
            command.CommandText <- "SELECT data FROM authorization_codes"
        
            use! reader = command.ExecuteReaderAsync() |> Async.AwaitTask
            return seq {
                while reader.Read() do yield JsonConvert.DeserializeObject<AuthorizationCode>(reader.GetString(0), Json.jsonOptions)
            }
        }
    
    let storeToken (key : string) (value : Token) =
        async {
            use connection = authentication()
            use command = connection.CreateCommand()

            command.CommandText <- "INSERT INTO tokens(id, data) VALUES (:id, :data) ON CONFLICT (id) DO UPDATE SET data = EXCLUDED.data"
            command.Parameters.AddWithValue("id", NpgsqlTypes.NpgsqlDbType.Varchar, key) |> ignore
            command.Parameters.AddWithValue("data", NpgsqlTypes.NpgsqlDbType.Jsonb, JsonConvert.SerializeObject(value, Json.jsonOptions)) |> ignore

            let! _ = command.ExecuteNonQueryAsync() |> Async.AwaitTask
            ()
        }

    let getToken (key : string) =
        async {
            use connection = authentication()
            use command = connection.CreateCommand()
        
            command.CommandText <- "SELECT data FROM tokens WHERE id = :id"
            command.Parameters.AddWithValue("id", NpgsqlTypes.NpgsqlDbType.Varchar, key) |> ignore
        
            use! reader = command.ExecuteReaderAsync() |> Async.AwaitTask
            match reader.Read() with
            | true  -> let result = JsonConvert.DeserializeObject<Token>(reader.GetString(0), Json.jsonOptions)
                       return Some result
            | false -> return None
        }

    let deleteToken (key : string) =
        async {
            use connection = authentication()
            use command = connection.CreateCommand()
        
            command.CommandText <- "DELETE FROM tokens WHERE id = :id"
            command.Parameters.AddWithValue("id", NpgsqlTypes.NpgsqlDbType.Varchar, key) |> ignore

            let! _ = command.ExecuteNonQueryAsync() |> Async.AwaitTask
            ()
        }

    let revokeToken (subject : string) (client : string) =
        async {
            use connection = authentication()
            use command = connection.CreateCommand()
        
            command.CommandText <- "DELETE FROM tokens WHERE data->>ClientId = :client AND data->>SubjectId = :subject"
            command.Parameters.AddWithValue("client", NpgsqlTypes.NpgsqlDbType.Varchar, client) |> ignore
            command.Parameters.AddWithValue("subject", NpgsqlTypes.NpgsqlDbType.Varchar, subject) |> ignore

            let! _ = command.ExecuteNonQueryAsync() |> Async.AwaitTask
            ()
        }
    
    let getTokens () =
        async {
            use connection = authentication()
            use command = connection.CreateCommand()
        
            command.CommandText <- "SELECT data FROM tokens"
        
            use! reader = command.ExecuteReaderAsync() |> Async.AwaitTask
            return seq {
                while reader.Read() do yield JsonConvert.DeserializeObject<Token>(reader.GetString(0), Json.jsonOptions)
            }
        }

    let storeRefreshToken (key : string) (value : RefreshToken) =
        async {
            use connection = authentication()
            use command = connection.CreateCommand()

            command.CommandText <- "INSERT INTO refresh_tokens(id, data) VALUES (:id, :data) ON CONFLICT (id) DO UPDATE SET data = EXCLUDED.data"
            command.Parameters.AddWithValue("id", NpgsqlTypes.NpgsqlDbType.Varchar, key) |> ignore
            command.Parameters.AddWithValue("data", NpgsqlTypes.NpgsqlDbType.Jsonb, JsonConvert.SerializeObject(value, Json.jsonOptions)) |> ignore

            let! _ = command.ExecuteNonQueryAsync() |> Async.AwaitTask
            ()
        }

    let getRefreshToken (key : string) =
        async {
            use connection = authentication()
            use command = connection.CreateCommand()
        
            command.CommandText <- "SELECT data FROM refresh_tokens WHERE id = :id"
            command.Parameters.AddWithValue("id", NpgsqlTypes.NpgsqlDbType.Varchar, key) |> ignore
        
            use! reader = command.ExecuteReaderAsync() |> Async.AwaitTask
            match reader.Read() with
            | true  -> let data = reader.GetString(0)
                       let result = JsonConvert.DeserializeObject<RefreshToken>(data, Json.jsonOptions)
                       return Some result
            | false -> return None
        }

    let deleteRefreshToken (key : string) =
        async {
            use connection = authentication()
            use command = connection.CreateCommand()
        
            command.CommandText <- "DELETE FROM refresh_tokens WHERE id = :id"
            command.Parameters.AddWithValue("id", NpgsqlTypes.NpgsqlDbType.Varchar, key) |> ignore

            let! _ = command.ExecuteNonQueryAsync() |> Async.AwaitTask
            ()
        }

    let revokeRefreshToken (subject : string) (client : string) =
        async {
            use connection = authentication()
            use command = connection.CreateCommand()
        
            command.CommandText <- "DELETE FROM refresh_tokens WHERE data->>ClientId = :client AND data->>SubjectId = :subject"
            command.Parameters.AddWithValue("client", NpgsqlTypes.NpgsqlDbType.Varchar, client) |> ignore
            command.Parameters.AddWithValue("subject", NpgsqlTypes.NpgsqlDbType.Varchar, subject) |> ignore

            let! _ = command.ExecuteNonQueryAsync() |> Async.AwaitTask
            ()
        }

    let getRefreshTokens () =
        async {
            use connection = authentication()
            use command = connection.CreateCommand()
        
            command.CommandText <- "SELECT data FROM refresh_tokens"
        
            use! reader = command.ExecuteReaderAsync() |> Async.AwaitTask
            return seq {
                while reader.Read() do yield JsonConvert.DeserializeObject<RefreshToken>(reader.GetString(0), Json.jsonOptions)
            }
        }

    let loadConsent (subject : string) (client : string) =
        async {
            use connection = authentication()
            use command = connection.CreateCommand()
        
            command.CommandText <- "SELECT data FROM consents WHERE clientid = :client AND subjectid = :subject"
            command.Parameters.AddWithValue("client", NpgsqlTypes.NpgsqlDbType.Varchar, client) |> ignore
            command.Parameters.AddWithValue("subject", NpgsqlTypes.NpgsqlDbType.Varchar, subject) |> ignore

            use! reader = command.ExecuteReaderAsync() |> Async.AwaitTask
            match reader.Read() with
            | true  -> let data = reader.GetString(0)
                       return Some (JsonConvert.DeserializeObject<Consent>(data, Json.jsonOptions))
            | false -> return None
        }
    
    let updateConsent (consent : Consent) =
        async {
            use connection = authentication()
            use command = connection.CreateCommand()

            command.CommandText <- """INSERT INTO
                                          consents(clientid, subjectid, data)
                                      VALUES (:client, :subject, :data)
                                      ON CONFLICT (id) DO UPDATE SET data = EXCLUDED.data"""
            command.Parameters.AddWithValue("client", NpgsqlTypes.NpgsqlDbType.Varchar, consent.ClientId) |> ignore
            command.Parameters.AddWithValue("subject", NpgsqlTypes.NpgsqlDbType.Varchar, consent.Subject) |> ignore
            command.Parameters.AddWithValue("data", NpgsqlTypes.NpgsqlDbType.Jsonb, JsonConvert.SerializeObject(consent, Json.jsonOptions)) |> ignore

            let! _ = command.ExecuteNonQueryAsync() |> Async.AwaitTask
            ()
        }
    
    let getConsents (subject : string) =
        async {
            use connection = authentication()
            use command = connection.CreateCommand()
            
            command.CommandText <- "SELECT data FROM consents WHERE subjectid = :subject"
            command.Parameters.AddWithValue("subject", NpgsqlTypes.NpgsqlDbType.Varchar, subject) |> ignore
            
            use! reader = command.ExecuteReaderAsync() |> Async.AwaitTask
            return seq {
                while reader.Read() do 
                    yield JsonConvert.DeserializeObject<Consent>(reader.GetString(0), Json.jsonOptions)
            }
        }

    let revokeConsent (subject : string) (client : string) =
        async {
            use connection = authentication()
            use command = connection.CreateCommand()
            
            command.CommandText <- "DELETE FROM consents WHERE clientid = :client AND subjectid = :subject"
            command.Parameters.AddWithValue("client", NpgsqlTypes.NpgsqlDbType.Varchar, client) |> ignore
            command.Parameters.AddWithValue("subject", NpgsqlTypes.NpgsqlDbType.Varchar, subject) |> ignore
    
            let! _ = command.ExecuteNonQueryAsync() |> Async.AwaitTask
            ()
        }
    
    let defaultStorage = {
        findUser = findUser;
        fetchUser = fetchUser;
        updateActive = updateActive;
        loadAllScopes = loadAllScopes;
        loadScopes = loadScopes;
        fetchClient = fetchClient;
    }

    let authorizationStorage = {
        storeAuthorizationCode = storeAuthorizationCode;
        getAuthorizationCode = getAuthorizationCode;
        deleteAuthorizationCode = deleteAuthorizationCode;
        revokeAuthorizationCode = revokeAuthorizationCode;
        getAuthorizationCodes = getAuthorizationCodes;
    }

    let tokenStorage = {
        storeToken = storeToken;
        getToken = getToken;
        deleteToken = deleteToken;
        revokeToken = revokeToken;
        getTokens = getTokens;
    }

    let refreshTokenStorage = {
        storeRefreshToken = storeRefreshToken;
        getRefreshToken = getRefreshToken;
        deleteRefreshToken = deleteRefreshToken;
        revokeRefreshToken = revokeRefreshToken;
        getRefreshTokens = getRefreshTokens;
    }

    let consentStorage = {
        loadConsent = loadConsent;
        updateConsent = updateConsent;
        getConsents = getConsents;
        revokeConsent = revokeConsent;
    }