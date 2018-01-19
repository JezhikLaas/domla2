namespace D2.Authentication

open D2.Common
open IdentityServer4.Services
open IdentityServer4.Events
open IdentityServer4.Extensions
open IdentityServer4.Stores
open Microsoft.AspNetCore.Authentication
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging

[<Route("[controller]")>]
[<SecurityHeaders>]
type AccountController
     (
         interaction : IIdentityServerInteractionService,
         users : UserStorage,
         events : IEventService,
         grantStore : PersistedGrantStorage,
         logger : ILogger<AccountController>,
         authorizer : Authorizer
     ) =
    inherit Controller()

    [<HttpGet("login")>]
    member this.Get (returnUrl : string) =
        async {
            let request = sprintf "/app/login?returnUrl=%s" (returnUrl.Base64UrlEncode())
            logger.LogDebug request
            return RedirectResult (request)
        }
        |> Async.StartAsTask

    [<HttpPost("login")>]
    [<ValidateAntiForgeryToken>]
    member this.Post (model : LoginInputModel) =
        async {
            logger.LogDebug (sprintf "login requested, starting to authenticate %s" model.Username)
            let! result = users.findUser model.Username model.Password
            match result with
            | None   -> logger.LogWarning (sprintf "failed to authenticate %s" model.Username)
                        return StatusCodeResult (StatusCodes.Status403Forbidden)
                        :> IActionResult
            
            | Some s -> logger.LogDebug (sprintf "authentication for %s succeeded" model.Username)
                        events.RaiseAsync(new UserLoginSuccessEvent(s.Login, s.Id.ToString("N"), s.Login))
                        |> Async.AwaitTask
                        |> Async.RunSynchronously

                        match interaction.IsValidReturnUrl model.ReturnUrl with
                        | true  -> this.HttpContext.SignInAsync (s.Id.ToString("N"), s.Login)
                                   |> Async.AwaitTask
                                   |> Async.RunSynchronously
                                   return authorizer.Authorize (this.HttpContext)

                        | false -> return StatusCodeResult (StatusCodes.Status403Forbidden)
                                   :> IActionResult
        }
        |> Async.StartAsTask

    [<HttpGet("logout")>]
    member this.Logout () =
        async {
            let user = this.HttpContext.User
            if user <> null then
                if user.Identity.IsAuthenticated = true then
                    this.HttpContext.SignOutAsync()
                    |> Async.AwaitTask
                    |> Async.RunSynchronously

                    events.RaiseAsync(new UserLogoutSuccessEvent(user.GetSubjectId(), user.GetDisplayName()))
                    |> Async.AwaitTask
                    |> Async.RunSynchronously

                    users.updateActive (user.Identity.GetSubjectId()) false
                    |> Async.RunSynchronously

                    do! grantStore.removeAll (user.Identity.GetSubjectId()) "interactive"
        
            return RedirectResult ("/app/goodbye")
        }
        |> Async.StartAsTask
