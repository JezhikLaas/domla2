namespace D2.Authentication

open Microsoft.AspNetCore.Mvc

type HomeController
     (
     ) =
    inherit Controller()

    member this.Index () =
        async {
            return VirtualFileResult ("~/index.html", "text/html")
        }
        |> Async.StartAsTask
