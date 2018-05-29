namespace D2.Welcome

open D2.Common
open System
open System.Collections.Generic
open System.IO
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Logging
open NLog
open NLog.Web

module Program =

    let BuildWebHost args =
        WebHost
            .CreateDefaultBuilder(args)
            .UseKestrel(fun options -> ServiceConfiguration.configureKestrel options)
            .UseWebRoot("wwwroot")
            .UseStartup<Startup>()
            .ConfigureLogging(
                fun logging -> logging.ClearProviders()
                               |> ignore
                               
                               logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace)
                               |> ignore
            )
            .UseNLog()
            .Build()

    [<EntryPoint>]
    let main args =
        let logConfig = Path.Combine(Directory.GetCurrentDirectory(), "nlog.config")
        if File.Exists logConfig then
            let logger = LogManager.LoadConfiguration(logConfig).GetCurrentClassLogger()
            logger.Info "logging configured"
        
        try
            BuildWebHost(args).Run()
            0
        with
        | _ -> 1
