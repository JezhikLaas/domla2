namespace D2.UI

module Program =

    open D2.Common
    open Microsoft.AspNetCore
    open Microsoft.AspNetCore.Hosting
    open NLog.Web
    open System

    let exitCode = 0

    let BuildWebHost args =
        WebHost
            .CreateDefaultBuilder(args)
            .UseKestrel(fun options -> ServiceConfiguration.configureKestrel options)
            .UseWebRoot("wwwroot")
            .UseStartup<Startup>()
            .UseNLog()
            .Build()

    [<EntryPoint>]
    let main args =
        let logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger()
        logger.Debug("init main")
        BuildWebHost(args).Run()

        exitCode
