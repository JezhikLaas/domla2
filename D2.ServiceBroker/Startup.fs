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
    let bearerEvents =
        let result = JwtBearerEvents()
        result.OnTokenValidated <- fun context -> 
                                       match context.SecurityToken with
                                       | :? JwtSecurityToken as token -> match context.Principal.Identity with 
                                                                         | :? ClaimsIdentity as identity -> identity.AddClaim (Claim("access_token", token.RawData))
                                                                         | _                             -> ()
                                       | _                            -> ()
                                       
                                       Task.CompletedTask
        result
    
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
            .AddIdentityServerAuthentication(
                fun options -> options.Authority <- (ServiceConfiguration.authority().FullAddress)
                               options.RequireHttpsMetadata <- false
                               options.ApiName <- "api"
                               options.ApiSecret <- "78C2A2A1-6167-45E4-A9D7-46C5D921F7D5"
                               options.SaveToken <- true
                               options.EnableCaching <- true
                               options.CacheDuration <- TimeSpan.FromMinutes 10.0
                               options.JwtBearerEvents <- bearerEvents
                               options.SupportedTokens <- SupportedTokens.Both
            )
            |> ignore

    member this.Configure(app: IApplicationBuilder, env: IHostingEnvironment) =
        app
            .UseCors("default")
            .UseAuthentication()
            .UseMvc()
            |> ignore

    member val Configuration : IConfiguration = null with get, set