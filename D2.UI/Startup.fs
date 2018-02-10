namespace D2.UI

open D2.Common
open System.Collections.Generic
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.HttpOverrides
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open System.IdentityModel.Tokens.Jwt


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
            .AddInteractiveAuthentication()
            |> ignore

    member this.Configure(app: IApplicationBuilder, env: IHostingEnvironment) =
        
        if env.EnvironmentName <> "Development" then
            app.UseForwardedHeaders(
                ForwardedHeadersOptions(
                    ForwardedHeaders = (
                        ForwardedHeaders.XForwardedFor ||| ForwardedHeaders.XForwardedProto
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