namespace D2.UserManagement.Controllers

open D2.Common
open D2.UserManagement
open D2.UserManagement.Persistence
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging

[<Route("[controller]")>]
type UsersController
    (
        logger : ILogger<UsersController>
    ) =
    inherit Controller()

    [<HttpPut("register")>]
    member this.Put() =
        let user = this.Request.Body.AsUtf8String()
        match Registration.register user with
        | Success Ok        -> StatusCodeResult(StatusCodes.Status200OK)
                               :> ActionResult
        | Success Known     -> StatusCodeResult(StatusCodes.Status301MovedPermanently)
                               :> ActionResult
        | Success Conflict  -> StatusCodeResult(StatusCodes.Status409Conflict)
                               :> ActionResult
        | InternalFailure e -> logger.LogError(e, "register")
                               StatusCodeResult(StatusCodes.Status500InternalServerError)
                               :> ActionResult
        | ExternalFailure s -> logger.LogInformation s
                               StatusCodeResult(StatusCodes.Status422UnprocessableEntity)
                               :> ActionResult
