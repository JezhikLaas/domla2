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

    [<ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)>]
    member this.Index () =
        async {
            return VirtualFileResult ("~/index.html", "text/html")
        }
        |> Async.StartAsTask
    
    [<Authorize(Roles = "admin")>]
    member this.Logout () =
        let user = this.HttpContext.User
        if user |> isNull |> not then
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
    
    member this.LoadConfiguration () =
        ContentResult(
            ContentType = "",
            Content = sprintf """{
                "stsServer": "%s",
                "client_id":"adminclient",
                "response_type":"id_token token",
                "scope":"openid profile role.profile"
            }""" (ServiceConfiguration.authority.FullAddress)
        )
            
