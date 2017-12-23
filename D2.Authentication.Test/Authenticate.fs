namespace D2Authentication.Test

open D2.Authentication
open FsUnit
open IdentityModel.Client
open IdentityServer4.Models
open NUnit.Framework
open RestSharp
open System
open System.Collections.Generic
open System.Net
open System.Security.Claims

[<TestFixture>]
[<Category("Authentication")>]
module AuthenticationTest = 
    let serverUrl = sprintf "%s://%s:%d" "http" "localhost" 8120
    let tokenUrl = sprintf "%s://%s:%d/connect/token" "http" "localhost" 8120
    let mutable (web : Object) = null
    let userId = Guid.NewGuid()
    let fejaClaims =
        [
            new Claim("email", "olga.laas@domla.de")
        ]
    
    let findUser (name : string) (password : string) =
        async {
            match name = "feja" && password = "geheim" with
            | true -> return Some {
                          new User with
                              member this.Id = userId
                              member this.Login = "feja"
                              member this.FirstName = "Olga"
                              member this.LastName = "Laas"
                              member this.EMail = "olga.laas@domla.de"
                              member this.Password = "geheim"
                              member this.Claims = fejaClaims
                              member this.LoggedIn = false
                              member this.Salutation = "Mrs"
                              member this.Title = String.Empty
                      }
            | false -> return None
        }
            
    let fetchUser (id : string) =
        async {
            match id = userId.ToString() with
            | true -> return Some {
                          new User with
                              member this.Id = userId
                              member this.Login = "feja"
                              member this.FirstName = "Olga"
                              member this.LastName = "Laas"
                              member this.EMail = "olga.laas@domla.de"
                              member this.Password = "geheim"
                              member this.Claims = fejaClaims
                              member this.LoggedIn = true
                              member this.Salutation = "Mrs"
                              member this.Title = String.Empty
                      }
            | false -> return None
        }
            
    let updateActive (id : string) (state : bool) =
        async { return () }
    (*
    let defaultScopes = [
            StandardScopes.Address;
            StandardScopes.AddressAlwaysInclude;
            StandardScopes.AllClaims;
            StandardScopes.Email;
            StandardScopes.EmailAlwaysInclude;
            StandardScopes.OfflineAccess;
            StandardScopes.OpenId;
            StandardScopes.ProfileAlwaysInclude;
            StandardScopes.Roles;
            StandardScopes.RolesAlwaysInclude;
            new Scope(Name = "user");
            new Scope(Name = "api");
        ]
    
    let loadAllScopes () =
        async {
            return defaultScopes |> List.toSeq
        }
    
    let loadScopes (names : string seq) =
        async {
            return defaultScopes |> Seq.filter(fun ds -> names |> Seq.contains(ds.Name))
        }

    let defaultClients =
        let users = new Client()
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

        [users; services]
    
    let fetchClient (id : string) =
        async {
            match defaultClients |> List.tryFind(fun c -> c.ClientId = id) with
            | Some client -> return Some client
            | None        -> return None
        }
    
    [<OneTimeSetUp>]
    let setupOnce() =
        Startup.testingMode <- true
        Startup.storageService <-
            {
                findUser = findUser;
                fetchUser = fetchUser;
                updateActive = updateActive;
                loadAllScopes = loadAllScopes;
                loadScopes = loadScopes;
                fetchClient = fetchClient
            }
        web <- WebApp.Start<Startup>(serverUrl)
    
    [<OneTimeTearDown>]
    let tearDownOnce() =
        web.Dispose()

    [<Test>]
    let ``Token for API can be fetched``() =
        let client = new TokenClient(tokenUrl, "silicon", "1B0A7C32-1A60-4D5D-AE4C-4163F72E467D")
        let result = client.RequestClientCredentialsAsync("api").Result

        result |> should not' (equal null)
        result.HttpStatusCode |> should equal HttpStatusCode.OK
        result.AccessToken |> should not' (equal null)

    [<Test>]
    let ``Token for User can be fetched``() =
        let client = new TokenClient(tokenUrl, "interactive", "0A0C7C53-1A60-4D5D-AE4C-4163F72E467D")
        let result = client.RequestResourceOwnerPasswordAsync("feja", "geheim", "user openid").Result

        result |> should not' (equal null)
        result.HttpStatusCode |> should equal HttpStatusCode.OK
        result.AccessToken |> should not' (equal null)

    [<Test>]
    let ``User can request refresh token``() =
        let client = new TokenClient(tokenUrl, "interactive", "0A0C7C53-1A60-4D5D-AE4C-4163F72E467D")
        let result = client.RequestResourceOwnerPasswordAsync("feja", "geheim", "user offline_access").Result
        
        result |> should not' (equal null)
        result.HttpStatusCode |> should equal HttpStatusCode.OK
        result.RefreshToken |> should not' (equal null)
    
    type TokenRequest = { token : string }
    
    [<Test>]
    let ``User token can be validated``() =
        let client = new TokenClient(tokenUrl, "interactive", "0A0C7C53-1A60-4D5D-AE4C-4163F72E467D")
        let result = client.RequestResourceOwnerPasswordAsync("feja", "geheim", "user offline_access openid").Result
        
        let validationClient = new RestClient(sprintf "%s/connect" serverUrl)
        let validationRequest = new RestRequest("accesstokenvalidation", Method.POST)
        validationRequest.AddObject({ token = result.AccessToken }) |> ignore

        let validation = validationClient.Execute(validationRequest)
        
        validation |> should not' (equal null)
        validation.StatusCode |> should equal HttpStatusCode.OK
    
    [<Test>]
    let ``Invalid access token is rejected``() =
        let validationClient = new RestClient(sprintf "%s/connect" serverUrl)
        let validationRequest = new RestRequest("accesstokenvalidation", Method.POST)
        validationRequest.AddObject({ token = "random" }) |> ignore

        let validation = validationClient.Execute(validationRequest)
        
        validation |> should not' (equal null)
        validation.StatusCode |> should equal HttpStatusCode.BadRequest
      
    [<Test>]
    let ``User info can be fetched``() =
        let client = new TokenClient(tokenUrl, "interactive", "0A0C7C53-1A60-4D5D-AE4C-4163F72E467D")
        let result = client.RequestResourceOwnerPasswordAsync("feja", "geheim", "user offline_access openid all_claims").Result
        
        let validationClient = new RestClient(sprintf "%s/connect" serverUrl)
        let infoRequest = (new RestRequest("userinfo", Method.GET)).AddHeader("Authorization", "Bearer " + result.AccessToken)
        infoRequest.RequestFormat <- DataFormat.Json

        let validation = validationClient.Execute(infoRequest)
        
        validation |> should not' (equal null)
        validation.StatusCode |> should equal HttpStatusCode.OK
        validation.Content.Length |> should be (greaterThan 2L)
    
    [<Test>]
    let ``User token can be refreshed``() =
        let client = new TokenClient(tokenUrl, "interactive", "0A0C7C53-1A60-4D5D-AE4C-4163F72E467D")
        let tokenResponse = client.RequestResourceOwnerPasswordAsync("feja", "geheim", "user offline_access openid").Result

        let result = client.RequestRefreshTokenAsync(tokenResponse.RefreshToken).Result
        
        result |> should not' (equal null)
        result.HttpStatusCode |> should equal HttpStatusCode.OK
        result.RefreshToken |> should not' (equal null)
*)