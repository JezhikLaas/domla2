namespace D2.Administration.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Authorization
open D2.Administration

[<Route("[controller]")>]
type RegistrationsController () =
    inherit Controller()

    [<HttpGet("list")>]
    [<Authorize>]
    member this.List () =
        async {
            let! response = CompositionRoot.Storage.listPending ()
            return JsonResult(response)
        }