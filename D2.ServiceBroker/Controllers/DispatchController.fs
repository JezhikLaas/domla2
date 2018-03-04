namespace D2.ServiceBroker

open D2.Common
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging

[<Route("[controller]")>]
type DispatchController
    (
        logger : ILogger<DispatchController>
    ) =
    inherit Controller()
    
    [<Authorize>]
    [<HttpGet("{name}/{version:regex(^(\\d\\d)$)}/{service}")>]
    member this.Get(name : string, version : int, service : string) =
        Response.emit (ResolveRoutes.endpoints name version service) logger
