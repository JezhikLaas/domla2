﻿namespace D2.ServiceBroker

open D2.Common
open D2.Service.Contracts.Common
open D2.Service.Contracts.Validation
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open Microsoft.AspNetCore.Http
open System
open System.Collections.Generic
open System.Linq
open System.Security.Claims

[<Route("[controller]")>]
type DispatchController
    (
        logger : ILogger<DispatchController>
    ) =
    inherit Controller()

    let argumentNames = [|"groups"; "topic"; "call"|]
    
    let concatErrorMessages (errors : Error []) =
        let descriptions = errors |> Array.map(fun error -> error.property + ": " + error.description)
        String.Join(" ; ", descriptions)
    
    let claimsToContext (claims : Claim seq) =
        let result = new Dictionary<string, string>()
        for claim in claims do
            if not (result.ContainsKey claim.Type) then
                result.Add (claim.Type, claim.Value)
        result
    
    [<Authorize>]
    [<HttpPost>]
    member this.Post(groups : string, topic : string, call : string) =
        this.Modify("Post", groups, topic, call)
    
    [<Authorize>]
    [<HttpPut>]
    member this.Put(groups : string, topic : string, call : string) =
        this.Modify("Put", groups, topic, call)

    member this.Modify(verb : string, groups : string, topic : string, call : string) =
        logger.LogDebug (sprintf "starting %s request" verb)
        let context = claimsToContext this.HttpContext.User.Claims
        let body = if this.Request.Body <> null then this.Request.Body.AsUtf8String () else null
        let parameterKeys = this.HttpContext.Request.Query.Keys.Where(argumentNames.Contains >> not)

        let parameters = seq {
            for key in parameterKeys do
                yield new Parameter(key, this.HttpContext.Request.Query.[key].ToString())
        }

        let request = Request(
                          topic,
                          verb,
                          call,
                          body,
                          parameters |> Seq.toArray
                      )

        try
            let result = ServiceConnection.validateAndExecute (groups.Split(',')) request context
            match result.Execution with
            | null ->   match result.Validation.result with
                        | State.ExternalFailure -> result.Validation.errors |> concatErrorMessages |> logger.LogWarning
                                                   this.StatusCode(StatusCodes.Status422UnprocessableEntity)
                                                   :> IActionResult
                        | _                     -> result.Validation.errors |> concatErrorMessages |> logger.LogError
                                                   this.StatusCode(StatusCodes.Status500InternalServerError)
                                                   :> IActionResult
        
            | result -> match String.IsNullOrWhiteSpace result.json with
                        | true  -> logger.LogDebug (sprintf "succeeded with %d" result.code)
                                   this.StatusCode result.code
                                   :> IActionResult
                        | false -> logger.LogDebug (sprintf "succeeded with %d and result %s" result.code result.json)
                                   this.Content(result.json, "application/json")
                                   :> IActionResult
        with
            error -> logger.LogError(verb + ": ", error)
                     this.StatusCode(StatusCodes.Status422UnprocessableEntity)
                     :> IActionResult

    [<Authorize>]
    [<HttpGet>]
    member this.Get(groups : string, topic : string, call : string) =
        logger.LogDebug "starting log request"
        let context = claimsToContext this.HttpContext.User.Claims
        let parameterKeys = this.HttpContext.Request.Query.Keys.Where(argumentNames.Contains >> not)

        let parameters = seq {
            for key in parameterKeys do
                yield new Parameter(key, this.HttpContext.Request.Query.[key].ToString())
        }

        let request = Request(
                          topic,
                          "Get",
                          call,
                          null,
                          parameters |> Seq.toArray
                      )
        try
            let result = ServiceConnection.execute (groups.Split(',')) request context
            match result.Execution with
            | null ->   match result.Validation.result with
                        | State.ExternalFailure -> result.Validation.errors |> concatErrorMessages |> logger.LogWarning
                                                   this.StatusCode(StatusCodes.Status422UnprocessableEntity)
                                                   :> IActionResult
                        | _                     -> result.Validation.errors |> concatErrorMessages |> logger.LogError
                                                   this.StatusCode(StatusCodes.Status500InternalServerError)
                                                   :> IActionResult
        
            | result -> match String.IsNullOrWhiteSpace result.json with
                        | true  -> logger.LogDebug (sprintf "succeeded with %d" result.code)
                                   this.StatusCode result.code
                                   :> IActionResult
                        | false -> logger.LogDebug (sprintf "succeeded with %d and result %s" result.code result.json)
                                   this.Content(result.json, "application/json")
                                   :> IActionResult
        with
            error -> logger.LogError("GET: ", error)
                     this.StatusCode(StatusCodes.Status500InternalServerError)
                     :> IActionResult
