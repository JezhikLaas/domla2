namespace D2.Authentication

open IdentityServer4.Events
open IdentityServer4.Extensions
open IdentityServer4.Services
open IdentityServer4.Validation
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
         authorizer : Authorizer,
         requestValidator : IUserInfoRequestValidator
     ) =
    inherit Controller()

    [<HttpGet("login")>]
    [<ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)>]
    member this.Get (returnUrl : string) =
        async {
            logger.LogDebug (sprintf "invoking login view with returnUrl %s" returnUrl)
            let request = sprintf "/_auth/login?encodedReturnUrl=%s" (returnUrl.Base64UrlEncode())
            logger.LogDebug request
            return RedirectResult (request)
        }
        |> Async.StartAsTask

    [<HttpPost("login")>]
    [<AutoValidateAntiforgeryToken>]
    member this.Post (model : LoginInputModel) =
        async {
            logger.LogDebug (sprintf "login requested, starting to authenticate %s" model.Username)
            let! result = users.findUser model.Username model.Password
            let parameters = this.HttpContext.Request.Query.AsNameValueCollection ()
            logger.LogDebug (parameters.ToString())
            match result with
            | None   -> logger.LogWarning (sprintf "failed to authenticate %s" model.Username)
                        return StatusCodeResult (StatusCodes.Status403Forbidden)
                        :> IActionResult
            
            | Some s -> logger.LogDebug (sprintf "authentication for %s succeeded" model.Username)
                        do! events.RaiseAsync(new UserLoginSuccessEvent(s.Login, s.Id.ToString("N"), s.Login))
                            |> Async.AwaitTask

                        let isValidReturnUrl = interaction.IsValidReturnUrl model.ReturnUrl
                                               ||
                                               interaction.IsValidReturnUrl (model.ReturnUrl.HtmlDecode ())

                        match isValidReturnUrl with
                        | true  -> do! this.HttpContext.SignInAsync (s.Id.ToString("N"), s.Login)
                                       |> Async.AwaitTask
                                   return authorizer.Authorize (this.HttpContext) (model.ReturnUrl.HtmlDecode ())

                        | false -> logger.LogWarning (sprintf "checking return url %s (%s) failed" model.ReturnUrl (model.ReturnUrl.HtmlDecode ()))
                                   return StatusCodeResult (StatusCodes.Status403Forbidden)
                                   :> IActionResult
        }
        |> Async.StartAsTask
    
    member this.DetermineUser (logoutId : string) =
        async {
            let token = match this.HttpContext.Request.Headers.TryGetValue ("Authorization") with
                        | true, value -> value.[0].Substring(7)
                        | false, _    -> null
            
            match token with
            | null -> return this.HttpContext.User
            | _    -> let! validation = requestValidator.ValidateRequestAsync token |> Async.AwaitTask
                      match validation = null || validation.IsError with
                      | true  -> return null
                      | false -> return validation.Subject
        }

    [<HttpGet("logout")>]
    member this.Logout (logoutId : string) =
        async {
            logger.LogDebug(sprintf "logging out %s" (if logoutId = null then "<null>" else logoutId))
            let! user = this.DetermineUser logoutId
            
            match user |> isNull && logoutId |> isNotNull with
            | false -> let! context = interaction.GetLogoutContextAsync logoutId
                                      |> Async.AwaitTask
                                      
                       do! this.HttpContext.SignOutAsync()
                           |> Async.AwaitTask

                       do! events.RaiseAsync(new UserLogoutSuccessEvent(context.SubjectId, user.GetDisplayName()))
                           |> Async.AwaitTask

                       do! users.updateActive (context.SubjectId) false
                       do! grantStore.removeAll (context.SubjectId) (context.ClientId)
        
                       let result = this.HttpContext.Request.Scheme
                                    +
                                    "://"
                                    +
                                    this.HttpContext.Request.Host.ToUriComponent()
                                    +
                                    "/_auth/goodbye"
                       return RedirectResult (result)
                              :> IActionResult

            
            | true  -> return StatusCodeResult (StatusCodes.Status200OK)
                              :> IActionResult
        }
        |> Async.StartAsTask
