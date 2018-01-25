namespace D2.ServiceBroker

open System
open D2.Common
open System.Collections.Generic
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection

open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Authorization.Infrastructure
open Microsoft.AspNetCore.Identity
open System.Threading.Tasks
open Microsoft.EntityFrameworkCore
open Microsoft.AspNetCore.Identity.EntityFrameworkCore
open Microsoft.AspNetCore.Http

type AuthenticatedTestRequestMiddleware (next : RequestDelegate) =

    member this.Invoke (context: HttpContext) =
        Task.FromResult 0

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

type ApplicationDbContext (options : DbContextOptions<ApplicationDbContext>) =
    inherit IdentityDbContext<ApplicationUser> (options)

type StartupTesting private () =
    new (configuration: IConfiguration) as this =
        StartupTesting() then
        this.Configuration <- configuration

    member this.ConfigureServices(services: IServiceCollection) =
        services.AddDbContext<ApplicationDbContext>(
            (fun options -> options.UseInMemoryDatabase(Guid.NewGuid().ToString()) |> ignore),
            ServiceLifetime.Singleton,
            ServiceLifetime.Singleton
        )
        |> ignore

        services
            .AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
        |> ignore

        services.Configure<IdentityOptions>(
            fun (options : IdentityOptions) -> options.Password.RequireDigit <- false
                                               options.Password.RequiredLength <- 4
                                               options.Password.RequireNonAlphanumeric <- false
                                               options.Password.RequireUppercase <- false
                                               options.Password.RequireLowercase <- false
                                               options.Password.RequiredUniqueChars <- 1
                                               options.Lockout.DefaultLockoutTimeSpan <- TimeSpan.FromMinutes(30.0)
                                               options.Lockout.MaxFailedAccessAttempts <- 10
                                               options.Lockout.AllowedForNewUsers <- true
                                               options.User.RequireUniqueEmail <- false
        )
        |> ignore
        
        services.ConfigureApplicationCookie(
            fun options -> options.Cookie.HttpOnly <- false
                           options.SlidingExpiration <- true
        )
        |> ignore
        
        services.AddMvc() |> ignore

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
            //.AddDefaultTokenProviders()
            |> ignore
        (*
        services.ConfigureApplicationCookie(fun options ->
            options.Events <- CookieAuthenticationEvents(
                                  OnRedirectToLogin = fun context ->
                                                          context.RedirectUri <- ""
                                                          Task.FromResult 0 :> Task
                              )
        )
        |> ignore
        *)
    member this.Configure(app: IApplicationBuilder, env: IHostingEnvironment) =
        app
            .UseCors("default")
            .UseAuthentication()
            .UseMiddleware<AuthenticatedTestRequestMiddleware>()
            .UseMvc()
            |> ignore

    member val Configuration : IConfiguration = null with get, set