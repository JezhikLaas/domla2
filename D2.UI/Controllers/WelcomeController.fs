namespace D2.UI.Controllers

open D2.Common
open Microsoft.AspNetCore.Authentication
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Server.HttpSys
open Microsoft.Extensions.Logging

type WelcomeController
     (
        logger : ILogger<HomeController>
     ) =
    inherit Controller()

    member this.Index () =
        this.View()