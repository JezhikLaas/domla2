namespace D2.Authentication

open IdentityServer4.Events
open IdentityServer4.Extensions
open IdentityServer4.ResponseHandling
open IdentityServer4.Services
open IdentityServer4.Validation
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open System
open System.Security.Claims
open System.Web

type Authorizer
     (
         events : IEventService,
         logger : ILogger<Authorizer>,
         validator : IAuthorizeRequestValidator,
         authorizeResponseGenerator : IAuthorizeResponseGenerator,
         userSession : IUserSession,
         users : UserStorage
     ) =
    
    member this.Authorize (context : HttpContext) (query : string) =
        logger.LogDebug "Start to authorize"

        let authorize parameters (user : ClaimsPrincipal) =
            match user = null with
            | true  -> logger.LogDebug "no user present in authorize request"
            | false -> logger.LogDebug (sprintf "user in authorize request: %s" (user.GetSubjectId ()))

            let result = validator.ValidateAsync (parameters, user)
                         |> Async.AwaitTask
                         |> Async.RunSynchronously
            
            match result.IsError with
            | true  ->  logger.LogWarning (sprintf "failed to authenticate %s" (user.GetSubjectId ()))
                        logger.LogWarning (sprintf "%s %s %s" result.Error Environment.NewLine result.ErrorDescription)
                        events.RaiseAsync(TokenIssuedFailureEvent(result.ValidatedRequest, result.Error, result.ErrorDescription))
                        |> Async.AwaitTask
                        |> Async.RunSynchronously

                        context.Response.Clear ()
                           
                        StatusCodeResult StatusCodes.Status403Forbidden
                        :> IActionResult
            
            | false -> let request = result.ValidatedRequest
                       let response = authorizeResponseGenerator.CreateResponseAsync request
                                      |> Async.AwaitTask
                                      |> Async.RunSynchronously

                       let responseData = AuthorizeResultModel (response)
                       
                       if response.IsError = false then
                           logger.LogDebug (sprintf "trigger success event for user %s" (user.GetSubjectId ()))
                           events.RaiseAsync (TokenIssuedSuccessEvent (response))
                           |> Async.AwaitTask
                           |> Async.RunSynchronously
                           
                           logger.LogDebug (sprintf "setting user %s to state ACTIVE" (user.GetSubjectId ()))
                           users.updateActive (user.GetSubjectId().ToString()) true
                           |> Async.RunSynchronously
                           
                           logger.LogDebug (sprintf "user %s authorized" (user.GetSubjectId ()))
                           logger.LogDebug (sprintf "return url %s is used" responseData.RedirectUri)
                       else
                           logger.LogError (sprintf "response for user %s indicates an error" (user.GetSubjectId ()))
                           logger.LogError (sprintf "%s %s %s" response.Error Environment.NewLine response.ErrorDescription)

                       JsonResult (responseData)
                       :> IActionResult

        match context.Request.Method with
        | "POST" -> let index = query.IndexOf('?')
                    let source = match index with
                                 | -1 -> query
                                 | _  -> query.Substring (index + 1)
                    let parameters = HttpUtility.ParseQueryString (source)
                    let user = userSession.GetUserAsync ()
                               |> Async.AwaitTask
                               |> Async.RunSynchronously
                    authorize parameters user

        | _      -> StatusCodeResult StatusCodes.Status405MethodNotAllowed
                    :> IActionResult
