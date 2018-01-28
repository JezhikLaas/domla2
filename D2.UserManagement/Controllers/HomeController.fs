namespace D2.UserManagement.Controllers

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
            logger.LogDebug "serving user management UI"
            return VirtualFileResult ("~/index.html", "text/html")
        }
        |> Async.StartAsTask


