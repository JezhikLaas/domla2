﻿namespace D2.Authentication

module SetupData =

    open BCrypt.Net
    open IdentityServer4
    open IdentityServer4.Models
    open Newtonsoft.Json
    open Npgsql
    open System
    open System.Security.Claims

    let initialize (options : ConnectionOptions) () =
        async {
            use connection = authentication options

            let insertOrUpdateClients () =
                use command = connection.CreateCommand ()
                let silicon = Client (
                                  ClientId = "service",
                                  AllowedGrantTypes = GrantTypes.ClientCredentials,
                                  ClientSecrets = [| Secret ("1B0A7C32-1A60-4D5D-AE4C-4163F72E467D".Sha256 ()) |],
                                  AllowedScopes = [| "api" |]
                              )
                let interactive = Client (
                                      ClientId = "interactive",
                                      ClientName = "Interactive user",
                                      AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                                      ClientSecrets = [| Secret ("0A0C7C53-1A60-4D5D-AE4C-4163F72E467D".Sha256 ()) |],
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
                                              (:id, :data)
                                          ON CONFLICT (id)
                                          DO UPDATE SET
                                              data = EXCLUDED.data"""
                    
                for client in [| silicon; interactive |] do
                    command.Parameters.Clear ()
                    command.Parameters << ("id", client.ClientId)
                                      <<< (
                        "data",
                        NpgsqlTypes.NpgsqlDbType.Jsonb,
                        JsonConvert.SerializeObject (client)
                    ) |> ignore
                    command.ExecuteNonQuery () |> ignore

            let insertOrUpdateIdentities () =
                use command = connection.CreateCommand ()
                command.CommandText <- """INSERT INTO
                                              identity_resources (name, data)
                                          VALUES
                                              (:name, :data)
                                          ON CONFLICT (name)
                                          DO UPDATE SET
                                              data = EXCLUDED.data"""
                    
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
                    command.Parameters << ("name", resource.Name)
                                      <<< (
                        "data",
                        NpgsqlTypes.NpgsqlDbType.Jsonb,
                        JsonConvert.SerializeObject (resource)
                    ) |> ignore
                    command.ExecuteNonQuery () |> ignore
                
            let insertOrUpdateApis () =
                use command = connection.CreateCommand ()
                command.CommandText <- """INSERT INTO
                                              api_resources (name, data)
                                          VALUES
                                              (:name, :data)
                                          ON CONFLICT (name)
                                          DO UPDATE SET
                                              data = EXCLUDED.data"""
                    
                let resources = 
                    [|
                        ApiResource ("api", "REST Api")
                    |]
                    
                for resource in resources do
                    command.Parameters.Clear ()
                    command.Parameters << ("name", resource.Name)
                                      <<< (
                        "data",
                        NpgsqlTypes.NpgsqlDbType.Jsonb,
                        JsonConvert.SerializeObject (resource)
                    ) |> ignore
                    command.ExecuteNonQuery () |> ignore
                
            let insertAdmin () =
                let isUserTableFilled () =
                    use command = connection.CreateCommand ()
                    command.CommandText <- """SELECT
                                                  CASE WHEN EXISTS (SELECT 1 FROM users)
                                                      THEN 1
                                                      ELSE 0
                                                  END"""
                
                    match command.ExecuteScalar () :?> int32 with
                    | 1 -> true
                    | _ -> false
                    
                match isUserTableFilled () with
                | true  -> ()
                | false -> let salt = BCrypt.GenerateSalt()
                           let password = BCrypt.HashPassword("secret", salt)

                           use insert = connection.CreateCommand()
                           insert.CommandText <- """INSERT INTO
                                                        users (id, login, password, last_name, email, claims)
                                                    VALUES
                                                        (:id, :login, :password, :last_name, :email, :claims)"""
                    
                           insert.Parameters << ("id", Guid.NewGuid())
                                             << ("login", "admin")
                                             << ("password", password)
                                             << ("last_name", "<unknown>")
                                             << ("email", "<unknown>")
                                            <<< ("claims", NpgsqlTypes.NpgsqlDbType.Jsonb, JsonConvert.SerializeObject([| new Claim("role", "admin") |]))
                                             |> ignore
                           insert.ExecuteNonQuery() |> ignore
                
            insertAdmin ()
            insertOrUpdateIdentities ()
            insertOrUpdateApis ()
            insertOrUpdateClients ()
        }

    let access (options : ConnectionOptions) =
        {
            initialize = initialize options;
        }
    
