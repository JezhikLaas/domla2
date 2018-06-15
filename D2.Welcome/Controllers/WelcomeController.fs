namespace D2.Welcome.Controllers

open D2.Welcome
open D2.Welcome.Types
open Microsoft.AspNetCore.Authentication
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open System

type WelcomeController
     (
        logger : ILogger<WelcomeController>
     ) =
    inherit Controller()
    
    member this.Finish ([<FromBody>] info : FinishRegistration) =
        async {
            let id = Guid (info.id)
            let! result = CompositionRoot.Storage.finish id info.password logger 
            match result with 
            | true  -> return StatusCodeResult(StatusCodes.Status202Accepted)
            | false -> return StatusCodeResult(StatusCodes.Status422UnprocessableEntity)
        }
        |> Async.StartAsTask
