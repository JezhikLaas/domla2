﻿namespace D2.ServiceBroker

open System.Collections.Generic
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open System.Security.Claims
open System.Threading.Tasks

type AuthenticatedTestRequestMiddleware (next : RequestDelegate) =

    let testingHeader = "X-Integration-Testing"
    let testingHeaderValue = "abcde-12345"
    let testingCookieAuthentication = "TestCookieAuthentication"

    member this.Invoke (context: HttpContext) =
        if context.Request.Headers.ContainsKey testingHeader then
            if context.Request.Headers.[testingHeader] |> Seq.contains testingHeaderValue then
                let name = context.Request.Headers.["user-name"] |> Seq.head
                let identity = ClaimsIdentity ([| Claim (ClaimTypes.Name, name) |], testingCookieAuthentication)
                context.User <- ClaimsPrincipal identity
        
        next.Invoke context
        |> Async.AwaitTask
        |> Async.RunSynchronously

        Task.FromResult 0

type StartupTesting private () =
    new (configuration: IConfiguration) as this =
        StartupTesting() then
        this.Configuration <- configuration

    member this.ConfigureServices(services: IServiceCollection) =
        services.AddMvc() |> ignore
    
    member this.Configure(app: IApplicationBuilder, env: IHostingEnvironment) =
        app
            .UseAuthentication()
            .UseMiddleware<AuthenticatedTestRequestMiddleware>()
            .UseMvc()
            |> ignore

    member val Configuration : IConfiguration = null with get, set