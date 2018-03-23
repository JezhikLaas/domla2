namespace D2.ServiceBroker

module ServiceConnection =
    open D2.Service.Contracts.Common
    open D2.Service.Contracts.Execution
    open D2.Service.Contracts.Validation
    open D2.ServiceBroker.Persistence.Mapper
    open Newtonsoft.Json.Linq
    open System
    open System.Collections.Generic

    type ExececutionResult (validation : ValidationResponse, execution : ExecutionResponse) =
        member this.Validation = validation
        member this.Execution = execution

    type ServiceConnector (service : ServiceI) as this =
        let communicator = Ice.Util.initialize()

        do
            this.Group <- service.Group
            let parts = service.BaseUrl.Split(':')
            
            let obj = communicator.stringToProxy(sprintf "Validator:default -h %s -p %s" parts.[0] parts.[1])
            this.Validator <- ValidatorPrxHelper.uncheckedCast(obj)

            let obj = communicator.stringToProxy(sprintf "Executor:default -h %s -p %s" parts.[0] parts.[1])
            this.Executor <- ExecutorPrxHelper.uncheckedCast(obj)
        
        member val private Executor : ExecutorPrx = null with get, set
        member val private Validator : ValidatorPrx = null with get, set
        member val Group = String.Empty with get, set

        member this.Dispose (disposing : bool) =
            if disposing then
                if not (isNull communicator) then communicator.Dispose ()
            ()

        interface IDisposable with
            member this.Dispose () =
                this.Dispose true
        
        member this.ValidateRequest (request : Request) =
            this.Validator.validateAsync request
                           |> Async.AwaitTask
                           |> Async.RunSynchronously
        
        member this.ExecuteRequest (request : Request) =
            this.Executor.executeAsync request
                          |> Async.AwaitTask
                          |> Async.RunSynchronously
            

    let private connectors = List<ServiceConnector>()
    
    let consolidateValidations (results : ValidationResponse seq) =
        let moreSeriousState left right =
            if left > right then left else right
        
        let joinValidation (left : ValidationResponse) (right : ValidationResponse) =
            new ValidationResponse(
                    (moreSeriousState left.result right.result),
                    Array.concat([left.errors; right.errors])
                )
        
        let head = results |> Seq.head
        let body = results |> Seq.tail
        body |> Seq.fold(joinValidation) head
    
    let consolidateExecutions (validation : ValidationResponse) (results : ExecutionResponse seq) =
        let grouped = results |> Seq.groupBy(fun result -> result.code)
        
        if grouped |> Seq.length > 1 then
            let errors = Array.concat(results |> Seq.map(fun result -> result.errors))
                         |>
                         Array.append([| (Error("", "inconsistent execution result")) |])
            
            let validationResult = new ValidationResponse(State.InternalFailure, errors)
            ExececutionResult (validationResult, null)
        else
            let notEmpty = String.IsNullOrWhiteSpace >> not
            let mergeJson left right =
                let first = JObject.Parse(left)
                let second = JObject.Parse(right)
                first.Merge(second)
                first.ToString()
            
            let json = results
                       |> Seq.filter(fun result -> result.json |> notEmpty)
                       |> Seq.fold(fun current result -> mergeJson current result.json) ""
            
            ExececutionResult (validation, ExecutionResponse(grouped |> Seq.head |> fst, json, [||]))
        
    let execute (groups : string seq) (request : Request) =
        lock connectors (fun () ->
            let matches = connectors |> Seq.where (fun connector -> groups |> Seq.contains connector.Group)
            for item in matches do
                item.ExecuteRequest request |> ignore
        )
    
    let validateAndExecute (groups : string seq) (request : Request) =
        lock connectors (fun () ->
            let matches = connectors
                          |> Seq.where (fun connector -> groups |> Seq.contains connector.Group)
                          |> Seq.toList

            let validations = seq {
                for item in matches do
                    yield item.ValidateRequest request
            }
            let validation = consolidateValidations validations

            if validation.result = State.NoError then
                let executions = seq {
                    for item in matches do
                        yield item.ExecuteRequest request
                    }
                consolidateExecutions validation executions
            else
                ExececutionResult (validation, null)
        )

    let initializeConnectors (services : ServiceI seq) =
        lock connectors (fun () -> 
            for connector in connectors do
                (connector :> IDisposable).Dispose()
        
            connectors.Clear()

            for service in services do
                connectors.Add (new ServiceConnector (service))
        )
