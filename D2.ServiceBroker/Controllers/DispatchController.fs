namespace D2.ServiceBroker

open D2.Common
open D2.Service.Contracts.Common
open D2.Service.Contracts.Validation
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open Microsoft.AspNetCore.Http
open System
open System.Linq

[<Route("[controller]")>]
type DispatchController
    (
        logger : ILogger<DispatchController>
    ) =
    inherit Controller()

    let argumentNames = [|"groups"; "topic"; "action"|]
    
    [<Authorize>]
    [<HttpPost>]
    member this.Post(groups : string, topic : string, action : string, [<FromBody>]body : string) =
        logger.LogDebug "starting log request"
        let parameterKeys = this.HttpContext.Request.Query.Keys.Where(argumentNames.Contains >> not)

        let parameters = seq {
            for key in parameterKeys do
                yield new Parameter(key, this.HttpContext.Request.Query.[key].ToString())
        }

        let request = Request(
                          topic,
                          "Post",
                          action,
                          body,
                          parameters |> Seq.toArray
                      )

        try
            let result = ServiceConnection.validateAndExecute (groups.Split(',')) request
            match result.Execution with
            | null ->   match result.Validation.result with
                        | State.ExternalFailure -> this.StatusCode(StatusCodes.Status422UnprocessableEntity)
                                                   :> IActionResult
                        | _                     -> this.StatusCode(StatusCodes.Status500InternalServerError)
                                                   :> IActionResult
        
            | result -> match String.IsNullOrWhiteSpace result.json with
                        | true  -> this.StatusCode result.code
                                   :> IActionResult
                        | false -> this.Content(result.json, "application/json")
                                   :> IActionResult
        with
            error -> logger.LogError("POST: ", error)
                     this.StatusCode(StatusCodes.Status422UnprocessableEntity)
                     :> IActionResult