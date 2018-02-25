namespace D2.Common

module ServiceRegistrator =

    open IdentityModel.Client
    open Microsoft.Extensions.Logging
    open System
    open System.Net
    open System.Net.Http
    open System.Net.Http.Headers
    open System.Text

    let registerSelf (logger : ILogger) (serializedService : string) =
        let tokenEndpoint =
            use discovery = new DiscoveryClient (ServiceConfiguration.authority.StandardAddress)
            if ServiceConfiguration.authority.Protocol = "http" then
                discovery.Policy.RequireHttps <- false

            logger.LogDebug (sprintf "Using url %s to contact authority" discovery.Url)
            
            let authorityInformation = discovery.GetAsync ()
                                       |> Async.AwaitTask
                                       |> Async.RunSynchronously

            match authorityInformation.IsError with
            | true  -> logger.LogCritical (
                           "Failed to contact authority: {0}",
                           authorityInformation.Error
                       )
                       null
            | false -> authorityInformation.TokenEndpoint
        
        let accessToken endpoint =
            match endpoint with
            | null -> null
            | _    -> use tokenClient = new TokenClient(
                                                endpoint,
                                                "service",
                                                "1B0A7C32-1A60-4D5D-AE4C-4163F72E467D"
                                            )
        
                      let tokenResponse = tokenClient.RequestClientCredentialsAsync("api")
                                         |> Async.AwaitTask
                                         |> Async.RunSynchronously

                      match tokenResponse.IsError with
                      | true  -> logger.LogCritical (
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
       
                   logger.LogDebug (sprintf "Trying to register with %s" url)
        
                   try
                       client.BaseAddress <- Uri url
                       client.DefaultRequestHeaders.Accept.Clear()
                       client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue("application/json"))
                       client.SetBearerToken token
        
                       let content = new StringContent(serializedService, Encoding.UTF8, "application/json")
        
                       logger.LogTrace (sprintf "Sending registration %s" serializedService)
        
                       let result = client.PutAsync("/apps/Domla2/01/register", content)
                                    |> Async.AwaitTask
                                    |> Async.RunSynchronously

                       logger.LogInformation (sprintf "Registration yielded %s" result.ReasonPhrase)
                       result.StatusCode = HttpStatusCode.OK
                   with
                   | error -> logger.LogError (error, "Failed to contact service broker")
                              false

