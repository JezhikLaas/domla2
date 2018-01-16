namespace D2.UI.Controllers

open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Mvc

type HomeController () =
    inherit Controller()

    [<Authorize>]
    member this.Index () =
        async {
            return VirtualFileResult ("~/index.html", "text/html")
        }
        |> Async.StartAsTask
