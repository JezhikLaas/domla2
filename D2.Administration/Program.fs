namespace D2.Administration

open D2.Common
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Hosting
open NLog.Web
open System.IO

module Program =
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
        let logConfig = Path.Combine(Directory.GetCurrentDirectory(), "nlog.config")
        if File.Exists logConfig then
            let logger = NLogBuilder.ConfigureNLog(logConfig).GetCurrentClassLogger()
            logger.Info "logging configured"
        BuildWebHost(args).Run()
        
        exitCode
