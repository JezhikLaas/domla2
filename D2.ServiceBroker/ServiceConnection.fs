namespace D2.ServiceBroker

module ServiceConnection =
    open D2.Service.Contracts.Common
    open D2.Service.Contracts.Execution
    open D2.Service.Contracts.Validation
    open D2.ServiceBroker.Persistence.Mapper
    open System
    open System.Collections.Generic

    type ServiceConnector (service : ServiceI) as this =
        let communicator = Ice.Util.initialize()

        let makeRequest (topic: string) (action : string) (body : string) (parameters : (string * string) seq) =
            let arguments = parameters
                            |> Seq.map(fun parameter -> new Parameter(fst parameter, snd parameter))
                            |> Seq.toArray
            Request(
                topic,
                "Post",
                action,
                body,
                arguments
            )

        do
            let parts = service.BaseUrl.Split(':')
            
            let obj = communicator.stringToProxy(sprintf "Validator:default -h %s -p %s" parts.[0] parts.[1])
            this.Validator <- ValidatorPrxHelper.uncheckedCast(obj)

            let obj = communicator.stringToProxy(sprintf "Executor:default -h %s -p %s" parts.[0] parts.[1])
            this.Executor <- ExecutorPrxHelper.uncheckedCast(obj)
        
        member val Executor : ExecutorPrx = null with get, set
        member val Validator : ValidatorPrx = null with get, set

        member this.Dispose (disposing : bool) =
            if disposing then
                if not (isNull communicator) then communicator.Dispose ()
            ()

        interface IDisposable with
            member this.Dispose () =
                this.Dispose true

        member this.Post (topic: string) (action : string) (body : string) (parameters : (string * string) seq) =
            let request = makeRequest topic action body parameters

            this.Executor.executeAsync request
                          |> Async.AwaitTask
                          |> Async.RunSynchronously

        member this.ValidatePost (topic: string) (action : string) (body : string) (parameters : (string * string) seq) =
            let request = makeRequest topic action body parameters

            this.Validator.validateAsync request
                           |> Async.AwaitTask
                           |> Async.RunSynchronously

    let connectors = List<ServiceConnector>()

    let initializeConnectors (services : ServiceI seq) =
        for connector in connectors do
            (connector :> IDisposable).Dispose()
        
        connectors.Clear()

        for service in services do
            connectors.Add (new ServiceConnector (service))
