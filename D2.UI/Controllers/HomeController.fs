namespace D2.UI.Controllers

open D2.Common
open Microsoft.AspNetCore.Authentication
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Server.HttpSys
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
    
    [<Authorize>]
    member this.Logout () =
        let user = this.HttpContext.User
        if user |> isNotNull then
            logger.LogInformation (sprintf "user %s attempts to sign off" user.Identity.Name)
        else
            logger.LogWarning "anonymous user attempts to sign off"

        async {
            do! this.HttpContext.SignOutAsync("Cookies")
                |> Async.AwaitTask
            
            do! this.HttpContext.SignOutAsync("oidc")
                |> Async.AwaitTask
        }
        |> Async.StartAsTask

    [<Authorize>]
    member this.Services () =
        async {
            logger.LogDebug "Starting service services broker request"
            let serviceBroker = ServiceConfiguration.services
            
            return ContentResult(
                       Content = sprintf """{"Broker": "%s"}""" serviceBroker.FullAddress,
                       ContentType = "application/json"
                   )
        }
        |> Async.StartAsTask
    
    member this.LoadConfiguration () =
        logger.LogDebug "providing oidc configuration"
        ContentResult(
            ContentType = "application/json",
            Content = sprintf """{
                "issuer": "%s",
                "clientId":"customer-frontend",
                "responseType":"id_token token",
                "scope":"openid profile role.profile api",
                "redirectUri": "%s"
            }""" (ServiceConfiguration.authority.StandardAddress) (ServiceConfiguration.configuration.PublicUrl)
        )

    [<HttpGet>]
    member this.Welcome (id : string) =
        this.View("Welcome")

    [<HttpPost>]
    member this.Password (id : string, password : string) =
        ()