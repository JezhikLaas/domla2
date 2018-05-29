namespace D2.Welcome.Controllers

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
