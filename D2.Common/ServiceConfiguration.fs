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
        member this.FullAddress
            with get () =
                sprintf "%s://%s:%d" this.Protocol this.Address this.Port
        member this.StandardAddress
            with get () =
                if this.Protocol = "http" && this.Port = 80 then
                    sprintf "%s://%s" this.Protocol this.Address
                else
                    if this.Protocol = "https" && this.Port = 443 then
                        sprintf "%s://%s" this.Protocol this.Address
                    else
                        sprintf "%s://%s" this.Protocol this.Address
    
    type AuthorityProperties () =
        inherit ServiceAddress ()
    
    type BrokerProperties () =
        inherit ServiceAddress ()
    
    type Service () = 
        member val Hosting = List<ServiceAddress>() with get, set
    
    type SelfProperties () =
        member val Version = 0 with get, set
        member val Patch = 0 with get, set

    type DatabaseProperties () =
        member val Identifier = String.Empty with get, set
        member val Name = String.Empty with get, set
        member val Host = String.Empty with get, set
        member val User = String.Empty with get, set
        member val Password = String.Empty with get, set
        member val Port = 0 with get, set
    
    type MappingEntry () =
        member val From = String.Empty with get, set
        member val To = String.Empty with get, set
    
    type ClientRedirects () = 
        member val Mappings = List<MappingEntry>() with get, set
    
    let configurationSources =
        let builder = ConfigurationBuilder()
        builder.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
               .AddJsonFile("appsettings.json")
               .AddJsonFile("appsettings.Development.json", true)
               .AddJsonFile("appsettings.qa.json", true)
               .AddEnvironmentVariables () |> ignore
        builder.Build ()
    
    let configuration =
        let config = Service ()
        configurationSources.GetSection("Service").Bind config

        config
    
    let authority =
        let evaluate () =
            let config = AuthorityProperties ()
            configurationSources.GetSection("Authority").Bind config

            config
        evaluate ()
    
    let services =
        let evaluate () =
            let config = BrokerProperties ()
            configurationSources.GetSection("ServiceBroker").Bind config

            config
        evaluate ()
    
    let versionInfo =
        let evaluate () =
            let config = SelfProperties ()
            configurationSources.GetSection("Self").Bind config

            config
        evaluate ()
    
    let connectionInfo =
        let evaluate () =
            let config = DatabaseProperties ()
            configurationSources.GetSection("Database").Bind config

            config
        evaluate ()
    
    let clientMappings =
        let evaluate () =
            let config = ClientRedirects ()
            configurationSources.GetSection("ClientRedirects").Bind config

            config
        evaluate ()
    
    let configureKestrel (options : KestrelServerOptions) =
        for hosting in configuration.Hosting do
            match hosting.Protocol with
            | "http"  -> let addresses = Dns.GetHostAddresses hosting.Address
                         for address in addresses do
                            options.Listen (
                                IPEndPoint (
                                    address,
                                    hosting.Port
                                )
                        )
            | _       -> failwith "unsupported protocol"
        ()