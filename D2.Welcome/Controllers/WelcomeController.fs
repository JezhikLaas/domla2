namespace D2.Welcome.Controllers

open D2.Welcome.Types
open Microsoft.AspNetCore.Authentication
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging

type WelcomeController
     (
        logger : ILogger<WelcomeController>
     ) =
    inherit Controller()
    
    member this.Finish ([<FromBody>] info : FinishRegistration) =
        async {
            return StatusCodeResult(StatusCodes.Status202Accepted)
        }
        |> Async.StartAsTask
