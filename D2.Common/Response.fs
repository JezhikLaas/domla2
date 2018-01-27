namespace D2.Common

open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc

module Response =
    open Microsoft.Extensions.Logging

    let emit f (logger : ILogger) = 
        match f with
        | Success x         -> ContentResult(
                                   Content = x,
                                   ContentType = "application/json"
                               )
                               :> ActionResult
        | InternalFailure e -> logger.LogError(e, "Internal error")
                               StatusCodeResult(StatusCodes.Status500InternalServerError) :> ActionResult
        | ExternalFailure s -> StatusCodeResult(StatusCodes.Status422UnprocessableEntity) :> ActionResult

    let confirm f (logger : ILogger) = 
        match f with
        | Success _         -> StatusCodeResult(StatusCodes.Status200OK) :> ActionResult
        | InternalFailure e -> logger.LogError(e, "Internal error")
                               StatusCodeResult(StatusCodes.Status500InternalServerError) :> ActionResult
        | ExternalFailure s -> StatusCodeResult(StatusCodes.Status422UnprocessableEntity) :> ActionResult
