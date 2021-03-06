namespace D2.UserManagement

open D2.Common
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.HttpOverrides
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Logging
open System
open System.Collections.Generic
open System.IdentityModel.Tokens.Jwt

type Startup private () =
    new (configuration: IConfiguration, logger: ILogger<Startup>) as this =
        Startup() then
        this.Configuration <- configuration
        if ServiceRegistration.registerSelf (logger) = false then
            logger.LogCritical "unable to register self"
            failwith "unable to register self"

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
                fun options -> options.Authority <- (ServiceConfiguration.authority.FullAddress)
                               options.EnableCaching <- true
                               options.ClientId <- "api"
                               options.ClientSecret <- "78C2A2A1-6167-45E4-A9D7-46C5D921F7D5"
                               options.SaveToken <- true
                               options.EnableCaching <- true
                               options.CacheDuration <- TimeSpan.FromMinutes 10.0
            )
            |> ignore

    member this.Configure(app: IApplicationBuilder, env: IHostingEnvironment) =
        
        if env.EnvironmentName <> "Development" then
            app.UseForwardedHeaders(
                ForwardedHeadersOptions(
                    ForwardedHeaders = (
                        ForwardedHeaders.All
                    )
                )
            )
            |> ignore
        
        app
            .UseCors("default")
            .UseAuthentication()
            .UseStaticFiles()
            .UseMvc(
                fun routes ->
                    routes.MapRoute(
                        name = "default", 
                        template = "{controller=Home}/{action=Index}/{id?}"
                    )
                    |> ignore
                    routes.MapSpaFallbackRoute(
                        name = "spa-fallback", 
                        defaults = { controller = "Home"; action = "Index" }
                    )
                    |> ignore
            )
            |> ignore

    member val Configuration : IConfiguration = null with get, set