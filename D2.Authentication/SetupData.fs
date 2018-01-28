namespace D2.Authentication

module SetupData =

    open IdentityModel
    open BCrypt.Net
    open D2.Common
    open IdentityServer4
    open IdentityServer4.Models
    open Npgsql
    open System
    open System.Security.Claims

    let private json = Json.Converter Json.jsonOptions

    let private initialize (options : ConnectionOptions) () =
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
                                      AccessTokenType = AccessTokenType.Reference,
                                      AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                                      ClientSecrets = [| Secret ("0A0C7C53-1A60-4D5D-AE4C-4163F72E467D".Sha256 ()) |],
                                      RedirectUris = [| "http://localhost:8130/signin-oidc" |],
                                      PostLogoutRedirectUris = [| "http://localhost:8130/signout-callback-oidc" |],
                                      RequireConsent = false,
                                      AlwaysIncludeUserClaimsInIdToken = true,
                                      AllowOfflineAccess = true,
                                      AllowAccessTokensViaBrowser = true,
                                      AlwaysSendClientClaims = true,
                                      AllowedScopes =
                                          [|
                                              IdentityServerConstants.StandardScopes.OpenId;
                                              IdentityServerConstants.StandardScopes.Profile;
                                              "role.profile";
                                              "api"
                                          |],
                                      AllowedCorsOrigins = [| "http://localhost:8130" |]
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
                    command.Parameters << ("id", StringField client.ClientId)
                                       << ("data", json.serialize client)
                                       |> ignore
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
                        IdentityResources.Email() :> IdentityResource;
                        IdentityResources.Phone() :> IdentityResource;
                        IdentityResources.Address() :> IdentityResource;
                    |]
                    
                for resource in resources do
                    command.Parameters.Clear ()
                    command.Parameters << ("name", StringField resource.Name)
                                       << ("data", json.serialize resource)
                                       |> ignore
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
                
                let apiResource = ApiResource ("api", "REST Api")
                apiResource.ApiSecrets <- [| Secret ("78C2A2A1-6167-45E4-A9D7-46C5D921F7D5".Sha256()) |]
                apiResource.UserClaims <- [| "name"; "role"; "id" |]

                let resources = 
                    [|
                        apiResource
                    |]
                    
                for resource in resources do
                    command.Parameters.Clear ()
                    command.Parameters << ("name", StringField resource.Name)
                                       << ("data", json.serialize resource)
                                       |> ignore
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
                           
                           insert.Parameters << ("id",  GuidField (Guid.NewGuid()))
                                             << ("login", StringField "admin")
                                             << ("password", StringField password)
                                             << ("last_name", StringField "<unknown>")
                                             << ("email", StringField "<unknown>")
                                             << ("claims", json.serialize [|
                                                                              new Claim(JwtClaimTypes.Role, "admin");
                                                                              new Claim(JwtClaimTypes.Name, "admin");
                                                                              new Claim(JwtClaimTypes.Id, Guid.NewGuid().ToString("N"));
                                                                          |]
                                                )
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
    

