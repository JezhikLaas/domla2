namespace D2.ServiceBroker

open D2.Common
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Identity
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Options
open System

type LoginModel () =
    member val Email = String.Empty with get, set
    member val Password = String.Empty with get, set

type RegisterModel () =
    member val Email = String.Empty with get, set
    member val Password = String.Empty with get, set

[<Route("[controller]")>]
type AccountController
    (
        signInManager : SignInManager<ApplicationUser>,
        userManager : UserManager<ApplicationUser>
    ) =
    inherit Controller ()

    [<HttpPost("Register")>]
    member this.Register ([<FromBody>]model : RegisterModel) =
        let user = ApplicationUser (UserName = model.Email, Email = model.Email)
        let result = userManager.CreateAsync (user, model.Password)
                     |> Async.AwaitTask
                     |> Async.RunSynchronously
        
        match result.Succeeded with
        | false -> StatusCodeResult (StatusCodes.Status422UnprocessableEntity)
                   :> IActionResult
        
        | true  -> StatusCodeResult (StatusCodes.Status200OK)
                   :> IActionResult

    [<HttpPost("Login")>]
    member this.Login ([<FromBody>]model : LoginModel, returnUrl : string) =
        let result = signInManager.PasswordSignInAsync (model.Email, model.Password, false, false)
                     |> Async.AwaitTask
                     |> Async.RunSynchronously
        
        match result.Succeeded with
        | false -> StatusCodeResult (StatusCodes.Status422UnprocessableEntity)
                   :> IActionResult
        
        | true  -> match String.IsNullOrWhiteSpace returnUrl with
                   | false -> this.Redirect returnUrl
                              :> IActionResult
                   
                   | true  -> StatusCodeResult (StatusCodes.Status200OK)
                              :> IActionResult
