namespace D2.ServiceBroker

open D2.Common
open Microsoft.AspNetCore.Mvc

[<Route("api/[controller]")>]
type AppsController () =
    inherit Controller()

    [<HttpGet>]
    member this.Get() =
        [|"value1"; "value2"|]

    [<HttpGet("{name}/(?<version>[\d]{2})/{service}")>]
    member this.Get(name : string, version : int, service : string) =
        Response.emit (ResolveRoutes.endpoints name version service)

    [<HttpPost>]
    member this.Post([<FromBody>]value:string) =
        ()

    [<HttpPut("{id}")>]
    member this.Put(id:int, [<FromBody>]value:string ) =
        ()

    [<HttpDelete("{id}")>]
    member this.Delete(id:int) =
        ()
