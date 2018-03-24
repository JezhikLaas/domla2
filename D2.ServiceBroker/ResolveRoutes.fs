namespace D2.ServiceBroker

open D2.Common
open D2.ServiceBroker.Persistence
open Newtonsoft.Json
open System

module ResolveRoutes =
    open Microsoft.Extensions.Logging

    let toJson items =
        handle {
            match items |> List.isEmpty with
            | true -> return! failExternal "no matching application / version"
            | false -> return! succeed (JsonConvert.SerializeObject(items))
        }
    
    let routes (name : string) (version : int) =
        let result = handle {
            let rawResult = CompositionRoot.Storage.routes name version |> Async.RunSynchronously
            return! toJson rawResult
        }
        
        result()

    let applications =
        let result = handle {
            let rawResult = CompositionRoot.Storage.applications () |> Async.RunSynchronously
            return! toJson rawResult
        }
        
        result()

    type EndPointI() =
        member val Name = String.Empty with get, set
        member val Uri = String.Empty with get, set

        interface EndPoint with
            member this.Name with get() = this.Name
            member this.Uri with get() = this.Uri

    type ServiceI() =
        member val Name = String.Empty with get, set
        member val BaseUrl = String.Empty with get, set
        member val Group = String.Empty with get, set
        member val Version = 0 with get, set
        member val Patch = 0 with get, set
        member val EndPoints = List.empty<EndPointI> with get, set
    
        interface Service with
            member this.Name with get() = this.Name
            member this.BaseUrl with get() = this.BaseUrl
            member this.Group with get() = this.Group
            member this.Version with get() = this.Version
            member this.Patch with get() = this.Patch
            member this.EndPoints with get() = this.EndPoints |> List.map(fun e -> e :> EndPoint)
    
    let register (name : string) (version : int) (service : string) (logger : ILogger) =
        let result = handle {
            let serviceItem = JsonConvert.DeserializeObject<ServiceI>(service)
            let result = CompositionRoot.Storage.register name version serviceItem |> Async.RunSynchronously
            let services = CompositionRoot.Storage.routes "Domla2" 1
                           |>
                           Async.RunSynchronously
                           |>
                           Seq.map(Persistence.Mapper.ServiceI.fromService)
        
            ServiceConnection.initializeConnectors services logger
            
            return result
        }
        
        result()
    
    let endpoints (name : string) (version : int) (service : string) =
        let result = handle {
            match CompositionRoot.Storage.endpoints name version service |> Async.RunSynchronously with
            | Some r -> return! succeed (JsonConvert.SerializeObject(r))
            | None   -> return! failExternal ("no service found")
        }
        
        result()
