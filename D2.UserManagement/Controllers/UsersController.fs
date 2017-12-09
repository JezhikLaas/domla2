namespace D2.UserManagement.Controllers

open D2.Common
open D2.UserManagement
open D2.UserManagement.Persistence
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc

[<Route("[controller]")>]
type UsersController () =
    inherit Controller()

    let logger = Logger.get "D2.UserManagement.Controllers.UsersController"

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
        | InternalFailure s -> logger.error s "register"
                               StatusCodeResult(StatusCodes.Status500InternalServerError)
                               :> ActionResult
        | ExternalFailure s -> logger.info s
                               StatusCodeResult(StatusCodes.Status422UnprocessableEntity)
                               :> ActionResult
