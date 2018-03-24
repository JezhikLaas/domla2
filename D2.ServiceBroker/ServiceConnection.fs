namespace D2.ServiceBroker

module ServiceConnection =
    open D2.Service.Contracts.Common
    open D2.Service.Contracts.Execution
    open D2.Service.Contracts.Validation
    open D2.ServiceBroker.Persistence.Mapper
    open Microsoft.Extensions.Logging
    open Newtonsoft.Json.Linq
    open System
    open System.Collections.Generic

    type ExececutionResult (validation : ValidationResponse, execution : ExecutionResponse) =
        member this.Validation = validation
        member this.Execution = execution
    
    let mutable logger : ILogger = null

    type ServiceConnector private () =
        
        new (service : ServiceI, logger : ILogger) as this =
            new ServiceConnector() then
            this.Communicator <- Ice.Util.initialize()
        
            this.Group <- service.Group
            let parts = service.BaseUrl.Split(':')
            
            let validator = sprintf "Validator:default -h %s -p %s" (parts.[1].TrimStart('/')) parts.[2]
            logger.LogDebug (sprintf "creating Validator %s" validator)
            let obj = this.Communicator.stringToProxy validator
            this.Validator <- ValidatorPrxHelper.uncheckedCast(obj)

            let executor = sprintf "Executor:default -h %s -p %s" (parts.[1].TrimStart('/')) parts.[2]
            logger.LogDebug (sprintf "creating Executor %s" executor)
            let obj = this.Communicator.stringToProxy executor
            this.Executor <- ExecutorPrxHelper.uncheckedCast(obj)

        member val private Communicator : Ice.Communicator = null with get, set
        member val private Executor : ExecutorPrx = null with get, set
        member val private Validator : ValidatorPrx = null with get, set
        member val Group = String.Empty with get, set

        member this.Dispose (disposing : bool) =
            if disposing then
                if not (isNull this.Communicator) then this.Communicator.Dispose ()
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
        
        else if grouped |> Seq.length = 0 then
            let validationResult = new ValidationResponse(State.InternalFailure, [| (Error("", "execution yielded no result")) |])
            ExececutionResult (validationResult, null)
        
        else
            let notEmpty = String.IsNullOrWhiteSpace >> not
            let mergeJson left right =
                match left with
                | "" -> right
                | _  -> let first = JObject.Parse(left)
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
            let executions = (
                    seq {
                        for item in matches do
                            yield item.ExecuteRequest request
                    } |> Seq.toList
                )
            consolidateExecutions (new ValidationResponse(State.NoError, [||])) executions
        )
    
    let validateAndExecute (groups : string seq) (request : Request) =
        lock connectors (fun () ->
            let matches = connectors
                          |> Seq.where (fun connector -> groups |> Seq.contains connector.Group)
                          |> Seq.toList

            let validations = (
                    seq {
                        for item in matches do
                            yield item.ValidateRequest request
                    } |> Seq.toList
                )
            let validation = consolidateValidations validations

            if validation.result = State.NoError then
                let executions = (
                        seq {
                            for item in matches do
                                item.ExecuteRequest request |> ignore
                        } |> Seq.toList
                    )
                consolidateExecutions validation executions
            else
                ExececutionResult (validation, null)
        )

    let initializeConnectors (services : ServiceI seq) (logger : ILogger) = 
        lock connectors (fun () -> 
            logger.LogDebug "starting conectors initialization"
            logger.LogDebug "clearing existing connectors"
            
            for connector in connectors do
                logger.LogDebug "disposing connector"
                (connector :> IDisposable).Dispose()
        
            connectors.Clear()

            for service in services do
                logger.LogDebug "adding connector"
                connectors.Add (new ServiceConnector (service, logger))
        )
