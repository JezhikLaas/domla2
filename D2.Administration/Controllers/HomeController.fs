namespace D2.Administration.Controllers

open D2.Common
open Microsoft.AspNetCore.Authentication
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging

type HomeController
     (
        logger : ILogger<HomeController>
     ) =
    inherit Controller()

    [<Authorize(Roles = "admin")>]
    [<ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)>]
    member this.Index () =
        async {
            logger.LogDebug "starting authorized request for Administration"
            let user = this.HttpContext.User
            if user <> null then
                logger.LogDebug (sprintf "user identified as %s" user.Identity.Name)
                let! accessToken = this.HttpContext.GetTokenAsync "access_token"
                                   |> Async.AwaitTask
                let! refreshToken = this.HttpContext.GetTokenAsync "refresh_token"
                                   |> Async.AwaitTask
                
                this.HttpContext.Response.Cookies.Append(
                    "access_token",
                    accessToken,
                    CookieOptions(
                        HttpOnly = false,
                        Secure = false
                    )
                )
                this.HttpContext.Response.Cookies.Append(
                    "refresh_token",
                    refreshToken,
                    CookieOptions(
                        HttpOnly = false,
                        Secure = false
                    )
                )
            return VirtualFileResult ("~/index.html", "text/html")
        }
        |> Async.StartAsTask
    
    [<Authorize(Roles = "admin")>]
    member this.Logout () =
        let user = this.HttpContext.User
        if user <> null then
            logger.LogInformation (sprintf "user %s attempts to sign off" user.Identity.Name)
        else
            logger.LogWarning "anonymous user attempts to sign off"

        async {
            do! this.HttpContext.SignOutAsync("Cookies")
                |> Async.AwaitTask
            
            do! this.HttpContext.SignOutAsync("oidc")
                |> Async.AwaitTask
            
            logger.LogDebug "signed out, returning"
        }
        |> Async.StartAsTask
