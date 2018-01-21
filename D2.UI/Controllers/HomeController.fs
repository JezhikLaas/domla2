namespace D2.UI.Controllers

open Microsoft.AspNetCore.Authentication
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Primitives

type HomeController () =
    inherit Controller()

    [<Authorize>]
    member this.Index () =
        async {
            let user = this.HttpContext.User
            if user <> null then
                let! accessToken = this.HttpContext.GetTokenAsync "access_token"
                                   |> Async.AwaitTask
                this.HttpContext.Response.Cookies.Append(
                    "access_token",
                    accessToken,
                    CookieOptions(
                        HttpOnly = false,
                        Secure = false
                    )
                )
            return VirtualFileResult ("~/index.html", "text/html")
        }
        |> Async.StartAsTask
    
    [<Authorize>]
    member this.Logout () =
        async {
            do! this.HttpContext.SignOutAsync("Cookies")
                |> Async.AwaitTask
            
            do! this.HttpContext.SignOutAsync("oidc")
                |> Async.AwaitTask
        }
        |> Async.StartAsTask

