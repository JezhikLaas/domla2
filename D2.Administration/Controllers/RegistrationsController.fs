namespace D2.Administration.Controllers

open D2.Administration
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Http

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

    [<HttpPost("confirm")>]
    [<Authorize>]
    member this.Confirm () =
        async {
            return this.StatusCode(StatusCodes.Status202Accepted);
        }