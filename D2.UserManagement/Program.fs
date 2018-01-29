namespace D2.UserManagement

open D2.Common
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Hosting
open NLog.Web
open System

module Program =
    let exitCode = 0

    let BuildWebHost args =
        WebHost
            .CreateDefaultBuilder(args)
            .UseKestrel(fun options -> ServiceConfiguration.configureKestrel options)
            .UseWebRoot("app")
            .UseStartup<Startup>()
            .UseNLog()
            .Build()

    [<EntryPoint>]
    let main args =
        if ServiceRegistration.registerSelf () then
            BuildWebHost(args).Run()
            0
        else
            1
