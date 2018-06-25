namespace D2.Common

open System

[<AutoOpen>]
module Request = 

    type RequestResult<'Result> =
    | Success of 'Result
    | ExternalFailure of string
    | InternalFailure of Exception

    type HandleRequest<'Result> = (unit -> RequestResult<'Result>)

    let succeed x = (fun () -> Success(x)) : HandleRequest<'Result>
    let failExternal s = (fun () -> ExternalFailure(s)) : HandleRequest<'Result>
    let failInternal s = (fun () -> InternalFailure(s)) : HandleRequest<'Result>
    let handleRequest (h : HandleRequest<'Result>) =
        try
            h()
        with
        | error -> InternalFailure(error)
    
    let bind h rest = match handleRequest h with
                      | Success r -> (rest r)
                      | ExternalFailure f -> failExternal f
                      | InternalFailure f -> failInternal f
    let delay h = (fun () -> handleRequest (try h() with | error -> failInternal error))

    type HandleRequestBuilder() =
        member b.Bind(h, rest) = bind h rest
        member b.Delay(h) = delay h
        member b.Return(h) = succeed h
        member b.ReturnFrom(h : HandleRequest<'Result>) = h
    
    let handle = HandleRequestBuilder()
    