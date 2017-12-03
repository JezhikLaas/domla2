namespace D2.ServiceBroker

open D2.Common
open Microsoft.AspNetCore.Mvc

[<Route("[controller]")>]
type AppsController () =
    inherit Controller()

    [<HttpGet("{name}/{version:regex(^([\d]{2})$)}/{service}")>]
    member this.Get(name : string, version : int, service : string) =
        Response.emit (ResolveRoutes.endpoints name version service)

    [<HttpPut("{name}/{version:regex(^([\d]{2})$)}/register")>]
    member this.Put(name : string, version : int, [<FromBody>] service : string) =
        Response.confirm (ResolveRoutes.register name version service)

    [<HttpGet("{name}/{version:regex(^([\d]{2})$)}")>]
    member this.Get(name : string, version : int) =
        Response.emit (ResolveRoutes.routes name version)
