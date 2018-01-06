namespace D2.Authentication

open D2.Common
open IdentityServer4.Services
open IdentityServer4.Events
open IdentityServer4.Stores
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc

[<Route("[controller]")>]
[<SecurityHeaders>]
type AccountController
     (
         interaction : IIdentityServerInteractionService,
         users : UserStorage,
         events : IEventService,
         clientStore : IClientStore,
         httpContextAccessor : IHttpContextAccessor
     ) =
    inherit Controller()

    let logger = Logger.get "D2.Authentication.Controllers.AccountController"
    let account = AccountService (interaction, clientStore, httpContextAccessor)

    [<HttpGet("login")>]
    member this.Get (returnUrl : string) =
        async {
            let model = account.BuildLoginViewModel returnUrl
            return RedirectToPageResult ("~/app")
        }
        |> Async.StartAsTask

    [<HttpPost("login")>]
    [<ValidateAntiForgeryToken>]
    member this.Post (model : LoginInputModel) =
        async {
            let! result = users.findUser model.Username model.Password
            match result with
            | None   -> return StatusCodeResult (StatusCodes.Status403Forbidden) :> ActionResult
            | Some s -> events.RaiseAsync(new UserLoginSuccessEvent(s.Login, s.Id.ToString("N"), s.Login))
                        |> Async.AwaitTask
                        |> Async.RunSynchronously

                        this.HttpContext.SignInAsync (s.Id.ToString("N"), s.Login)
                        |> Async.AwaitTask
                        |> Async.RunSynchronously

                        match interaction.IsValidReturnUrl model.ReturnUrl with
                        | true  -> return RedirectToPageResult (model.ReturnUrl) :> ActionResult
                        | false -> return RedirectToPageResult ("~/")            :> ActionResult
        }

    [<HttpPost("logout")>]
    [<ValidateAntiForgeryToken>]
    member this.Post (model : LogoutInputModel) =
        ()
