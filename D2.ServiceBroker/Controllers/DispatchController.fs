namespace D2.ServiceBroker

open D2.Common
open D2.Service.Contracts.Common
open D2.Service.Contracts.Validation
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open Microsoft.AspNetCore.Http
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

        let result = ServiceConnection.validateAndExecute (groups.Split(',')) request
        if result.Execution = null then
            match result.Validation.result with
            | State.ExternalFailure -> this.StatusCode(StatusCodes.Status422UnprocessableEntity)
            | _                     -> this.StatusCode(StatusCodes.Status500InternalServerError)
        else
            this.StatusCode(StatusCodes.Status422UnprocessableEntity)
