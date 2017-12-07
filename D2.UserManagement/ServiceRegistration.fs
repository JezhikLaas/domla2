namespace D2.UserManagement

open D2.Common
open Newtonsoft.Json
open System
open System.Net
open System.Net.Http
open System.Net.Http.Headers
open System.Text

module ServiceRegistration =
    let private logger = Logger.get "D2.UserManagement.ServiceRegistration"

    type EndPoint() =
        member val Name = String.Empty with get, set
        member val Uri = String.Empty with get, set

    type Service() =
        member val Name = "UserManagement" with get
        member val BaseUrl = sprintf "%s://%s:%d"
                                     configuration.Hosting.Protocol
                                     configuration.Hosting.Host
                                     configuration.Hosting.Port with get
        member val Version = configuration.Self.Version with get
        member val Patch = configuration.Self.Patch with get
        member val EndPoints = [|
                                   EndPoint(Name = "Frontend", Uri = "/");
                                   EndPoint(Name = "Register", Uri = "/users/register");
                               |] with get

    let registerSelf () =
        use client = new HttpClient()
        let url = sprintf "%s://%s:%d"
                          configuration.Broker.Protocol
                          configuration.Broker.Host
                          configuration.Broker.Port
        
        logger.debug(sprintf "Trying to register with %s" url)
        
        client.BaseAddress <- new Uri(url)
        client.DefaultRequestHeaders.Accept.Clear()
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"))
        
        let textData = JsonConvert.SerializeObject(new Service())
        let content = new StringContent(textData, Encoding.UTF8, "application/json")
        
        logger.trace(sprintf "Sending registration %s" textData)
        
        try
            let result = client.PutAsync("/apps/Domla2/01/register", content).Result
            logger.info(sprintf "Registration yielded %s" result.ReasonPhrase)
            result.StatusCode = HttpStatusCode.OK
        with
        | error -> logger.error error "Failed to contact service broker"
                   false
