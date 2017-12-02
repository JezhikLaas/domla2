namespace D2.ServiceBroker.Persistence

open Cast
open System

module Mapper = 
    
    type EndPointI() =
        member val Name = String.Empty with get, set
        member val Uri = String.Empty with get, set
        
        static member fromEndpoint (other : EndPoint) =
            new EndPointI(Name = other.Name, Uri = other.Uri)
    
        interface EndPoint with
            member this.Name with get() = this.Name
            member this.Uri with get() = this.Uri

    let private mapEndpoints (values : EndPoint list)=
        values |> List.map(fun ep -> new EndPointI(Name = ep.Name, Uri = ep.Uri))

    type ServiceI() =
        member val Name = String.Empty with get, set
        member val BaseUrl = String.Empty with get, set
        member val Version = 0 with get, set
        member val Patch = 0 with get, set
        member val EndPoints : EndPointI list = [] with get, set
        
        static member fromService (service : Service) =
            new ServiceI(
                    Name = service.Name.ToLowerInvariant(),
                    BaseUrl = service.BaseUrl,
                    Version = service.Version,
                    Patch = service.Patch,
                    EndPoints = mapEndpoints service.EndPoints
                )
    
        interface Service with
            member this.Name with get() = this.Name
            member this.BaseUrl with get() = this.BaseUrl
            member this.Version with get() = this.Version
            member this.Patch with get() = this.Patch
            member this.EndPoints with get() = this.EndPoints |> List.map(fun s -> s :> EndPoint)

    type ApplicationI() =
        member val Id = Guid.Empty with get, set
        member val Name = String.Empty with get, set
        member val Version = 0 with get, set
        member val Patch = 0 with get, set
        member val Services : ServiceI list = []  with get, set

        interface Application with
            member this.Id with get() = this.Id
            member this.Name with get() = this.Name
            member this.Version with get() = this.Version
            member this.Patch with get() = this.Patch
            member this.Services with get() = this.Services |> List.map(fun s -> s :> Service)
