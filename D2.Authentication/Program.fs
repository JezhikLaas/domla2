namespace D2.Authentication

module Program =

    open Microsoft.AspNetCore
    open Microsoft.AspNetCore.Hosting
    open System

    let exitCode = 0

    let BuildWebHost args =
        WebHost
            .CreateDefaultBuilder(args)
            .UseKestrel(fun options -> ServiceConfiguration.configureKestrel options)
            .UseWebRoot("app")
            .UseContentRoot(AppDomain.CurrentDomain.BaseDirectory)
            .UseStartup<Startup>()
            .Build()

    [<EntryPoint>]
    let main args =
        BuildWebHost(args).Run()

        exitCode
