namespace D2.Common

module ServiceConfiguration =

    open Microsoft.AspNetCore.Server.Kestrel.Core
    open Microsoft.Extensions.Configuration
    open System.Net
    open System
    open System.Collections.Generic

    type ServiceAddress () =
        member val Protocol = String.Empty with get, set
        member val Address = String.Empty with get, set
        member val Port = 0 with get, set
    
    type AuthorityProperties () =
        inherit ServiceAddress ()
            member this.FullAddress
                with get () =
                    sprintf "%s://%s:%d" this.Protocol this.Address this.Port
    
    type BrokerProperties () =
        inherit ServiceAddress ()
            member this.FullAddress
                with get () =
                    sprintf "%s://%s:%d" this.Protocol this.Address this.Port
    
    type Service () = 
        member val Hosting = List<ServiceAddress>() with get, set

    let configurationSources =
        let builder = ConfigurationBuilder()
        builder.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
               .AddJsonFile("appsettings.json")
               .AddJsonFile("appsettings.Development.json")
               .AddEnvironmentVariables () |> ignore
        builder.Build ()
    
    let configuration =
        let config = Service ()
        configurationSources.GetSection("Service").Bind config

        config
    
    let authority () =
        let config = AuthorityProperties ()
        configurationSources.GetSection("Authority").Bind config

        config
    
    let services () =
        let config = BrokerProperties ()
        configurationSources.GetSection("ServiceBroker").Bind config

        config
    
    let configureKestrel (options : KestrelServerOptions) =
        for hosting in configuration.Hosting do
            match hosting.Protocol with
            | "http"  -> options.Listen (
                            IPEndPoint (
                                IPAddress.Parse hosting.Address,
                                hosting.Port
                            )
                        )
            | _       -> failwith "unsupported protocol"
        ()