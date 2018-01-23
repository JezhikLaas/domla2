namespace D2.ServiceBroker

open System
open D2.Common
open System.Collections.Generic
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open System.IdentityModel.Tokens.Jwt

open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Authorization.Infrastructure
open Microsoft.AspNetCore.Identity
open System.Threading.Tasks
open Microsoft.AspNetCore.Authentication.Cookies


type DummyAuthorizationHandler () =
    inherit AuthorizationHandler<OperationAuthorizationRequirement> () with

        override this.HandleRequirementAsync
                      (
                        context : AuthorizationHandlerContext,
                        requirement : OperationAuthorizationRequirement
                      ) =
            context.Succeed requirement
            Task.FromResult(0)
            :> Task

type StartupTesting private () =
    new (configuration: IConfiguration) as this =
        StartupTesting() then
        this.Configuration <- configuration

    member this.ConfigureServices(services: IServiceCollection) =
        services.AddMvc() |> ignore
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear()
        services
            .AddCors(
                fun options -> options.AddPolicy(
                                   "default",
                                   fun policy ->
                                       policy.AllowAnyOrigin()
                                             .AllowAnyHeader()
                                             .AllowAnyMethod()
                                    |> ignore
                            )
            )
            .AddSingleton<IAuthorizationHandler, DummyAuthorizationHandler>()
            .AddIdentity<IdentityRole, IdentityRole>()
            .AddDefaultTokenProviders()
            |> ignore
        
        services.ConfigureApplicationCookie(fun options ->
            options.Events <- CookieAuthenticationEvents(
                                  OnRedirectToLogin = fun context ->
                                                          context.RedirectUri <- ""
                                                          Task.FromResult 0 :> Task
                              )
        )
        |> ignore

    member this.Configure(app: IApplicationBuilder, env: IHostingEnvironment) =
        app
            .UseCors("default")
            .UseMvc()
            .UseAuthentication()
            |> ignore

    member val Configuration : IConfiguration = null with get, set