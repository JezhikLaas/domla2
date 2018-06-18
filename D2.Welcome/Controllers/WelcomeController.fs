namespace D2.Welcome.Controllers

open D2.Common
open D2.Welcome
open D2.Welcome.Types
open Microsoft.AspNetCore.Authentication
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open System
open System.Text.RegularExpressions

type WelcomeController
     (
        logger : ILogger<WelcomeController>
     ) =
    inherit Controller()
    
    // this is the same pattern as it is used within the frontend code
    // if the business rule for passwords should be change, adjust both patterns
    let passwordPattern = Regex "^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,}"
    
    let finishRegistration (info : FinishRegistration) =
        let result = handle {
            let passwordCheck = passwordPattern.Match info.password
            match passwordCheck.Success with 
            | false -> return! failExternal "password does not match requirements"
            
            | true  -> let id = Guid (info.id)
                       match CompositionRoot.Storage.finish id info.password logger |> Async.RunSynchronously with 
                       | true  -> return! succeed ServiceConfiguration.login.StandardAddress
                       | false -> return! failExternal "storage failed"
        }
        
        result ()
    
    member this.Finish ([<FromBody>] info : FinishRegistration) =
        match finishRegistration info with
        | Success   address -> ContentResult(
                                   Content = (sprintf "{\"goto\": \"%s\"}" address),
                                   ContentType = "application/json"
                               )
                               :> ActionResult
        | InternalFailure e -> logger.LogError (e, "Internal error")
                               StatusCodeResult StatusCodes.Status500InternalServerError
                               :> ActionResult
        | ExternalFailure e -> logger.LogWarning e
                               StatusCodeResult StatusCodes.Status422UnprocessableEntity
                               :> ActionResult
