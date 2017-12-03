namespace D2.Common

open NLog
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc

module Response =

    let logger = LogManager.GetLogger ("D2.Common.Response")

    let emit f = 
        match f with
        | Success x -> JsonResult(x) :> ActionResult
        | InternalFailure s -> logger.Error(s)
                               StatusCodeResult(StatusCodes.Status500InternalServerError) :> ActionResult
        | ExternalFailure s -> StatusCodeResult(StatusCodes.Status422UnprocessableEntity) :> ActionResult

    let confirm f = 
        match f with
        | Success _ -> StatusCodeResult(StatusCodes.Status200OK) :> ActionResult
        | InternalFailure s -> logger.Error(s)
                               StatusCodeResult(StatusCodes.Status500InternalServerError) :> ActionResult
        | ExternalFailure s -> StatusCodeResult(StatusCodes.Status422UnprocessableEntity) :> ActionResult
