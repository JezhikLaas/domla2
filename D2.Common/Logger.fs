namespace D2.Common

module Logger =
    open NLog
    open System
    open System.Collections.Generic
    open System.IO

    let private configuration = new Config.XmlLoggingConfiguration(
                                        Path.Combine(
                                            AppDomain.CurrentDomain.BaseDirectory,
                                            "NLog.config"
                                        )
                                    )

    let private loggers =
        LogManager.Configuration <- configuration
        new Dictionary<Object, Logger>()

    let private trace (logger : Object) (msg : String)=
        loggers.[logger].Trace(msg)
        ()

    let private debug (logger : Object) (msg : String)=
        loggers.[logger].Debug(msg)
        ()

    let private info (logger : Object) (msg : String) =
        loggers.[logger].Info(msg)
        ()

    let private error (logger : Object) (ex : Exception) (msg : String) =
        loggers.[logger].Error(ex, msg)
        ()

    let private fatal (logger : Object) (msg : String)=
        loggers.[logger].Fatal(msg)
        ()

    type InternalLogger() =
        member this.trace (msg : string) =
            trace this msg
        member this.debug (msg : string) =
            debug this msg
        member this.info (msg : string) =
            info this msg
        member this.error(ex : Exception) (msg : string) =
            error this ex msg
        member this.fatal (msg : string) =
            fatal this msg

    let get name =
        let worker = LogManager.GetLogger(name)
        let result = new InternalLogger()
        loggers.[result] <- worker

        result
