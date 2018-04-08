namespace D2.Administration.Controllers

open D2.Administration
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Logging
open System

[<Route("[controller]")>]
type RegistrationsController
     (
        logger : ILogger<RegistrationsController>,
        configuration : IConfiguration
     ) =
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
    member this.Confirm (ids : Guid[]) =
        async {
            let response = Registration.acceptRegistrations ids logger
            return this.StatusCode(response)
        }