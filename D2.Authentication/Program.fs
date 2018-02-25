namespace D2.Authentication

module Program =

    open D2.Common
    open Microsoft.AspNetCore
    open Microsoft.AspNetCore.Hosting
    open Microsoft.Extensions.Logging
    open NLog.Web
    open System.IO

    let exitCode = 0

    let BuildWebHost args =
        WebHost
            .CreateDefaultBuilder(args)
            .UseKestrel(fun options -> ServiceConfiguration.configureKestrel options)
            .UseWebRoot("wwwroot")
            .UseStartup<Startup>()
            .ConfigureLogging(
                fun logging -> logging.ClearProviders()
                               |> ignore
                               
                               logging.SetMinimumLevel(LogLevel.Trace)
                               |> ignore
            )
            .UseNLog()
            .Build()

    [<EntryPoint>]
    let main args =
        let logConfig = Path.Combine(Directory.GetCurrentDirectory(), "nlog.config")
        if File.Exists logConfig then
            let logger = NLogBuilder.ConfigureNLog(logConfig).GetCurrentClassLogger()
            logger.Info "logging configured"
        
        BuildWebHost(args).Run()

        exitCode
