namespace D2.UserManagement

open Microsoft.AspNetCore
open Microsoft.AspNetCore.Hosting
open NLog.Web

module Program =
    let exitCode = 0

    let BuildWebHost args =
        WebHost
            .CreateDefaultBuilder(args)
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
