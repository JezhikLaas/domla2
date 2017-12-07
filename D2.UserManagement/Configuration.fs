namespace D2.UserManagement

open System
open Microsoft.Extensions.Configuration

[<AutoOpen>]
module Configuration =

    type ServiceAddress () =
        member val Protocol = String.Empty with get, set
        member val Host = String.Empty with get, set
        member val Port = 0 with get, set
    
    type VersionInfo () =
        member val Version = 0 with get, set
        member val Patch = 0 with get, set

    type Config () = 
        member val Hosting = ServiceAddress () with get
        member val Broker = ServiceAddress () with get
        member val Self = VersionInfo () with get

    let configuration =
        let builder = ConfigurationBuilder()
        builder.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
               .AddJsonFile("appsettings.json") |> ignore
        
        let configuration = builder.Build()
        let result = Config ()

        result.Broker.Host      <- configuration.GetValue<string>("Broker:Host")
        result.Broker.Protocol  <- configuration.GetValue<string>("Broker:Protocol")
        result.Broker.Port      <- configuration.GetValue<int>   ("Broker:Port")
        result.Hosting.Host     <- configuration.GetValue<string>("Hosting:Host")
        result.Hosting.Protocol <- configuration.GetValue<string>("Hosting:Protocol")
        result.Hosting.Port     <- configuration.GetValue<int>   ("Hosting:Port")
        result.Self.Version     <- configuration.GetValue<int>   ("Self:Version")
        result.Self.Patch       <- configuration.GetValue<int>   ("Self:Patch")

        result
