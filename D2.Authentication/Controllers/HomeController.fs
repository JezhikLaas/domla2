namespace D2.Authentication

open D2.Common
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Hosting
open IdentityServer4.Models
open IdentityServer4.Services

[<SecurityHeaders>]
type HomeController
     (
         interaction : IIdentityServerInteractionService,
         host : IHostingEnvironment
     ) =
    inherit Controller()

    member this.Index () =
        async {
            return VirtualFileResult ("~/index.html", "text/html")
        }
        |> Async.StartAsTask

    member this.Error (errorId : string) =
        async {
            let errorResult (error : string) (description : string) =
                match description = null with
                | false -> RedirectResult (
                               sprintf "/error?error=%s&description=%s"
                                       (error.Html())
                                       (description.Html())
                           )
                | true  -> RedirectResult (
                               sprintf "/error?error=%s" (error.Html())
                           )
            
            let message = interaction.GetErrorContextAsync(errorId)
                          |> Async.AwaitTask
                          |> Async.RunSynchronously

            match message = null with
            | false -> match host.IsDevelopment () with
                       | true  -> return errorResult message.Error message.ErrorDescription
                       | false -> return errorResult message.Error null
            
            | true  -> return errorResult "Unbekannter Fehler" null
        }
        |> Async.StartAsTask