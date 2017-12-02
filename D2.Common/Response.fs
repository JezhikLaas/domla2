namespace D2.Common

open NLog
open System.Net
open System.Text
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc

module Response =

    let logger = NLog.LogManager.GetLogger ("D2.Common.Response")

    let emit f = 
        match f with
        | Success x -> new JsonResult(x) :> ActionResult
        | InternalFailure s -> //logger.Error(s)
                               new StatusCodeResult(StatusCodes.Status500InternalServerError) :> ActionResult
        | ExternalFailure s -> new StatusCodeResult(StatusCodes.Status422UnprocessableEntity) :> ActionResult

    let confirm f = 
        match f with
        | Success _ -> new StatusCodeResult(StatusCodes.Status200OK) :> ActionResult
        | InternalFailure s -> // logger.Error(s)
                               new StatusCodeResult(StatusCodes.Status500InternalServerError) :> ActionResult
        | ExternalFailure s -> new StatusCodeResult(StatusCodes.Status422UnprocessableEntity) :> ActionResult
