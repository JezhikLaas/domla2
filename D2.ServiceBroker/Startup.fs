namespace D2.ServiceBroker

open System
open D2.Common
open System.Collections.Generic
open Microsoft.AspNetCore.Authentication.JwtBearer
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open System.IdentityModel.Tokens.Jwt
open System.Security.Claims
open System.Threading.Tasks
open IdentityServer4.AccessTokenValidation

type Startup private () =
    
    new (configuration: IConfiguration) as this =
        Startup() then
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
            .AddAuthentication("Bearer")
            .AddOAuth2Introspection(
                fun options -> options.Authority <- (ServiceConfiguration.authority().FullAddress)
                               options.EnableCaching <- true
                               options.ClientId <- "api"
                               options.ClientSecret <- "78C2A2A1-6167-45E4-A9D7-46C5D921F7D5"
                               options.SaveToken <- true
                               options.EnableCaching <- true
                               options.CacheDuration <- TimeSpan.FromMinutes 10.0
            )
            |> ignore

    member this.Configure(app: IApplicationBuilder, env: IHostingEnvironment) =
        app
            .UseCors("default")
            .UseAuthentication()
            .UseMvc()
            |> ignore

    member val Configuration : IConfiguration = null with get, set