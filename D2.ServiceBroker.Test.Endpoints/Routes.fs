namespace D2ServiceBrokerTestEndpoints

open D2.Common
open D2.ServiceBroker
open D2.ServiceBroker.Persistence
open FsUnit
open NUnit.Framework
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.TestHost
open System
open System.Collections.Generic
open System.Net.Http

[<TestFixture>]
[<Category("Endpoints")>]
module RoutesTest = 

    let private applicationRoutes (name : string) (version : int) =
        async {
            let entry = seq {
                if name = "Domla2" && version = 1 then
                    yield {
                        new Service with
                        member this.Name = "xxx"
                        member this.BaseUrl = "http://base"
                        member this.Version = 1
                        member this.Patch = 0
                        member this.EndPoints = []
                    }
            }
            return entry |> Seq.toList
        }

    let private applicationVersions =
        async {
            return [
                {
                    new Application with
                    member this.Id = Guid.NewGuid()
                    member this.Name = "Domla2"
                    member this.Version = 1
                    member this.Patch = 0
                    member this.Services = []
                }
            ]
        }
    
    type EndPointI() =
        member val Name = String.Empty with get, set
        member val Uri = String.Empty with get, set

        interface EndPoint with
            member this.Name with get() = this.Name
            member this.Uri with get() = this.Uri

    type ServiceI() =
        member val Name = String.Empty with get, set
        member val BaseUrl = String.Empty with get, set
        member val Version = 0 with get, set
        member val Patch = 0 with get, set
        member val EndPoints = List.empty<EndPointI> with get, set
    
        interface Service with
            member this.Name with get() = this.Name
            member this.BaseUrl with get() = this.BaseUrl
            member this.Version with get() = this.Version
            member this.Patch with get() = this.Patch
            member this.EndPoints with get() = this.EndPoints |> List.map(fun e -> e :> EndPoint)
    
    let storageFake = new Dictionary<string, Service>()

    let private registerService (target : (string * int)) (service : Service) =
        async {
            return storageFake.Add(service.Name.ToLowerInvariant(), service)
        }
    
    let private listEndpoints (target : (string * int)) (service : string) =
        async {
            match storageFake.TryGetValue service with
            | true, s  -> return Some s
            | false, _ -> return None
        }

    let testStorage = {
        services = applicationRoutes
        applications = applicationVersions
        register = registerService
        endpoints = listEndpoints
    }
    
    let mutable browser = null

    [<OneTimeSetUp>]
    let setupOnce () =
        CompositionRoot.setStorage testStorage
        let server = new TestServer((WebHostBuilder()).UseStartup<Startup>())
        browser <- server.CreateClient()

    [<SetUp>]
    let init () =
        storageFake.Clear()
    
    [<Test>]
    let ``Service broker answers 'Ok' when request is valid``() =
        let response = browser.GetAsync("/apps/Domla2/01/")
                       |> Async.AwaitTask
                       |> Async.RunSynchronously
        let result = response.StatusCode

        int result |> should equal StatusCodes.Status200OK
    
    [<Test>]
    let ``Service broker answers 'Unprocessable Entity' when application is unknown``() =
        let response = browser.GetAsync("/apps/Unknown/01/")
                       |> Async.AwaitTask
                       |> Async.RunSynchronously
        let result = response.StatusCode

        int result |> should equal StatusCodes.Status422UnprocessableEntity
    
    [<Test>]
    let ``Service broker lists known applications``() =
        let response = browser.GetAsync("/apps/list/")
                       |> Async.AwaitTask
                       |> Async.RunSynchronously
        let result = response.StatusCode

        int result |> should equal StatusCodes.Status200OK
    
    [<Test>]
    let ``Service broker answers 'Ok' when registering service``() =
        let service = ServiceI(
                          Name = "Test",
                          BaseUrl = "http://localhost:2300",
                          Version = 1,
                          Patch = 0,
                          EndPoints = []
                      )
        let body = Newtonsoft.Json.JsonConvert.SerializeObject(service)
        let response = browser.PutAsync("/apps/Domla2/01/register",  new StringContent(body, Text.Encoding.UTF8, "application/json"))
                       |> Async.AwaitTask
                       |> Async.RunSynchronously
        let result = response.StatusCode

        int result |> should equal StatusCodes.Status200OK
    
    [<Test>]
    let ``Service broker answers 'Ok' when registering service with Endpoints``() =
        let service = ServiceI(
                          Name = "Test",
                          BaseUrl = "http://localhost:2300",
                          Version = 1,
                          Patch = 0,
                          EndPoints = [
                              EndPointI(Name = "123", Uri = "/")
                          ]
                      )
        let body = Newtonsoft.Json.JsonConvert.SerializeObject(service)
        let response = browser.PutAsync("/apps/Domla2/01/register", new StringContent(body, Text.Encoding.UTF8, "application/json"))
                       |> Async.AwaitTask
                       |> Async.RunSynchronously
        let result = response.StatusCode

        int result |> should equal StatusCodes.Status200OK
    
    [<Test>]
    let ``Service broker delivers infos for specific service``() =
        let endpoint = EndPointI(Name = "123", Uri = "/")
        let service = ServiceI(
                          Name = "Test",
                          BaseUrl = "http://localhost:2300",
                          Version = 1,
                          Patch = 0,
                          EndPoints = [
                              endpoint
                          ]
                      )
        let body = Newtonsoft.Json.JsonConvert.SerializeObject(service)
        browser.PutAsync("/apps/Domla2/01/register", new StringContent(body, Text.Encoding.UTF8, "application/json"))
        |> Async.AwaitTask
        |> Async.RunSynchronously
        |> ignore

        let response = browser.GetAsync("/apps/Domla2/01/test")
                       |> Async.AwaitTask
                       |> Async.RunSynchronously
        
        int response.StatusCode |> should equal StatusCodes.Status200OK
        let text = response.Content.ReadAsStringAsync()
                   |> Async.AwaitTask
                   |> Async.RunSynchronously
        let answer = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceI>(
                                                     text
                                                 )
        answer.BaseUrl |> should equal "http://localhost:2300"
        answer.EndPoints.Length |> should equal 1
        answer.EndPoints.[0].Name |> should equal endpoint.Name
        answer.EndPoints.[0].Uri |> should equal endpoint.Uri

module Program = let [<EntryPoint>] main _ = 0