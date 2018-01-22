namespace D2.ServiceBroker

open System
open D2.Common
open System.Collections.Generic
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
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
            .AddAuthentication(fun options ->
                options.DefaultScheme <- "Cookies"
                options.DefaultChallengeScheme <- "oidc"
            )
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", fun options -> 
                options.SignInScheme <- "Cookies"

                options.Authority <- (ServiceConfiguration.authority().FullAddress)
                options.RequireHttpsMetadata <- false

                options.ClientId <- "interactive"
                options.ClientSecret <- "0A0C7C53-1A60-4D5D-AE4C-4163F72E467D"
                options.ResponseType <- "code id_token"

                options.SaveTokens <- true
                options.GetClaimsFromUserInfoEndpoint <- true

                options.Scope.Add("offline_access")
                options.Scope.Add("profile")
                options.Scope.Add("role.profile")
                options.Scope.Add("api")
            )   
            |> ignore

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    member this.Configure(app: IApplicationBuilder, env: IHostingEnvironment) =
        app
            .UseCors("default")
            .UseAuthentication()
            .UseMvc()
            |> ignore

    member val Configuration : IConfiguration = null with get, set