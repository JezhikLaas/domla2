namespace D2.UI.Controllers

open Microsoft.AspNetCore.Authentication
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging

type HomeController () =
    inherit Controller()

    [<Authorize>]
    member this.Index () =
        async {
            return VirtualFileResult ("~/index.html", "text/html")
        }
        |> Async.StartAsTask
    
    member this.Logout () =
        async {
            this.HttpContext.SignOutAsync("Cookies")
            |> Async.AwaitTask
            |> Async.RunSynchronously

            this.HttpContext.SignOutAsync("oidc")
            |> Async.AwaitTask
            |> Async.RunSynchronously
        }
        |> Async.StartAsTask

