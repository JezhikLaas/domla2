namespace D2.UserManagement

open D2.Common
open Newtonsoft.Json
open NLog
open System
open System.Net
open System.Net.Http
open System.Net.Http.Headers
open System.Text

module ServiceRegistration =
    open IdentityModel.Client

    let private logger = LogManager.GetLogger "D2.UserManagement.ServiceRegistration"

    type EndPoint() =
        member val Name = String.Empty with get, set
        member val Uri = String.Empty with get, set

    type Service() =
        member val Name = "UserManagement" with get
        member val BaseUrl = (
                              ServiceConfiguration.configuration.Hosting
                              |> Seq.head
                             ).FullAddress
                             
        member val Version = ServiceConfiguration.versionInfo.Version with get
        member val Patch = ServiceConfiguration.versionInfo.Patch with get
        member val EndPoints = [|
                                   EndPoint(Name = "Frontend", Uri = "/");
                                   EndPoint(Name = "Register", Uri = "/users/register");
                               |] with get

    let registerSelf () =
        let tokenEndpoint =
            use discovery = new DiscoveryClient (ServiceConfiguration.authority.FullAddress)
            let authorityInformation = discovery.GetAsync ()
                                       |> Async.AwaitTask
                                       |> Async.RunSynchronously

            match authorityInformation.IsError with
            | true  -> logger.Fatal (
                           "Failed to contact authority: {0}",
                           authorityInformation.Error
                       )
                       null
            | false -> authorityInformation.TokenEndpoint
        
        let accessToken endpoint =
            use tokenClient = new TokenClient(
                                  endpoint,
                                  "service",
                                  "1B0A7C32-1A60-4D5D-AE4C-4163F72E467D"
                              )
        
            let tokenResponse = tokenClient.RequestClientCredentialsAsync("api")
                                |> Async.AwaitTask
                                |> Async.RunSynchronously

            match tokenResponse.IsError with
            | true  -> logger.Fatal (
                           "Failed to authorize: {0} {1}",
                           tokenResponse.Error,
                           tokenResponse.ErrorDescription
                       )
                       null

            | false -> tokenResponse.AccessToken
         
        match tokenEndpoint |> accessToken with
        | null  -> false
        | token -> use client = new HttpClient()
                   let url = ServiceConfiguration.services.FullAddress
       
                   logger.Debug (sprintf "Trying to register with %s" url)
        
                   try
                       client.BaseAddress <- Uri url
                       client.DefaultRequestHeaders.Accept.Clear()
                       client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue("application/json"))
                       client.SetBearerToken token
        
                       let textData = JsonConvert.SerializeObject(Service ())
                       let content = new StringContent(textData, Encoding.UTF8, "application/json")
        
                       logger.Trace (sprintf "Sending registration %s" textData)
        
                       let result = client.PutAsync("/apps/Domla2/01/register", content)
                                    |> Async.AwaitTask
                                    |> Async.RunSynchronously

                       logger.Info (sprintf "Registration yielded %s" result.ReasonPhrase)
                       result.StatusCode = HttpStatusCode.OK
                   with
                   | error -> logger.Error (error, "Failed to contact service broker")
                              false
