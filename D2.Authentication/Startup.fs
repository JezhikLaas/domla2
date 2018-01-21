namespace D2.Authentication

open D2.Common
open IdentityServer4.Models
open IdentityServer4.Services
open IdentityServer4.Stores
open Microsoft.AspNetCore.Antiforgery
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Logging
open System
open System.IO
open System.Threading.Tasks
open IdentityServer4.Configuration

type Startup private () =
    new (configuration: IConfiguration, loggerFactory : ILoggerFactory) as this =
        Startup() then
        this.Configuration <- configuration
        this.LoggerFactory <- loggerFactory

    // This method gets called by the runtime. Use this method to add services to the container.
    member this.ConfigureServices(services: IServiceCollection) =
        let connectionOptions = {
            Database = this.Configuration.GetValue<string>("Database:Name");
            Host = this.Configuration.GetValue<string>("Database:Host");
            User = this.Configuration.GetValue<string>("Database:User");
            Password = this.Configuration.GetValue<string>("Database:Password");
            Port = this.Configuration.GetValue<int>("Database:Port");
        }
        
        let setupStorage = Storage.storages.setupStorage connectionOptions;

        setupStorage.initialize () |> Async.RunSynchronously

        let persistedGrantStore = Storage.storages.persistedGrantStorage connectionOptions
        let resourceStore = Storage.storages.resourceStorage connectionOptions
        let clientStore = Storage.storages.clientStorage connectionOptions
        let userStore = Storage.storages.userStorage connectionOptions
        
        services.AddMvc()  |> ignore
        services
            .AddAntiforgery(
                fun options -> options.HeaderName <- "X-XSRF-TOKEN"
                               options.Cookie.HttpOnly <- false
                               options.Cookie.Path <- null
            )
            .AddCors(
                fun options -> options.AddPolicy(
                                   "default",
                                   fun policy ->
                                       policy.WithOrigins("http://localhost:8130")
                                             .AllowAnyHeader()
                                             .AllowAnyMethod()
                                    |> ignore
                            )
            )
        |> ignore

        let cors = DefaultCorsPolicyService (this.LoggerFactory.CreateLogger())
        cors.AllowAll <- true

        services
            .AddSingleton(clientStore)
            .AddSingleton(resourceStore)
            .AddSingleton(persistedGrantStore)
            .AddSingleton(userStore)
            .AddSingleton<ICorsPolicyService>(cors)
            .AddTransient<Authorizer>()
            .AddScoped<IPersistedGrantStore, PersistedGrantStore>()
            .AddSingleton<TokenCleanup>()
            .AddIdentityServer()
            .AddDeveloperSigningCredential()
            .AddClientStore<ClientStore>()
            .AddProfileService<ProfileService>()
            .AddResourceStore<ResourceStore>()
        |> ignore

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    member this.Configure(app: IApplicationBuilder, env: IHostingEnvironment, appLifetime : IApplicationLifetime, antiforgery : IAntiforgery) =
        let tokenMiddleware = fun (context : HttpContext) (next: Func<Task>) ->
                                  let path = context.Request.Path.Value
                                  if path <> null && not (path.ToLower().Contains("/api")) then
                                      let tokens = antiforgery.GetAndStoreTokens(context)
                                      context.Response.Cookies.Append("XSRF-TOKEN", 
                                          tokens.RequestToken, CookieOptions (
                                                                   HttpOnly = false, 
                                                                   Secure = false
                                                               )
                                      )
                                  next.Invoke ()
        app
            .UseCors("default")
            .UseStaticFiles()
            .UseIdentityServer()
            .Use(tokenMiddleware)
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
        
        let tokenCleanup = app.ApplicationServices.GetService<TokenCleanup>()
        appLifetime.ApplicationStarted.Register (fun () -> tokenCleanup.Start ()) |> ignore
        appLifetime.ApplicationStopping.Register (fun () -> tokenCleanup.Stop ()) |> ignore

    member val Configuration : IConfiguration = null with get, set
    member val LoggerFactory : ILoggerFactory = null with get, set