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
                             
        member val Version = ServiceConfiguration.versionInfo().Version with get
        member val Patch = ServiceConfiguration.versionInfo().Patch with get
        member val EndPoints = [|
                                   EndPoint(Name = "Frontend", Uri = "/");
                                   EndPoint(Name = "Register", Uri = "/users/register");
                               |] with get

    let registerSelf () =
        use client = new HttpClient()
        let url = ServiceConfiguration.services().FullAddress
        
        logger.Debug (sprintf "Trying to register with %s" url)
        
        client.BaseAddress <- Uri url
        client.DefaultRequestHeaders.Accept.Clear()
        client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue("application/json"))
        
        let textData = JsonConvert.SerializeObject(Service ())
        let content = new StringContent(textData, Encoding.UTF8, "application/json")
        
        logger.Trace (sprintf "Sending registration %s" textData)
        
        try
            let result = client.PutAsync("/apps/Domla2/01/register", content).Result
            logger.Info (sprintf "Registration yielded %s" result.ReasonPhrase)
            result.StatusCode = HttpStatusCode.OK
        with
        | error -> logger.Error (error, "Failed to contact service broker")
                   false
