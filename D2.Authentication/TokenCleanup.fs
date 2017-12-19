namespace D2.Authentication

open System.Threading

type TokenCleanup () =

    let source = new CancellationTokenSource()

    let clearOutdatedTokens () =
        ()

    let run =
        async {
            do! Async.Sleep(1000)
            clearOutdatedTokens ()
        }

    member this.Start () =
        Async.Start (run, source.Token)
    
    member this.Stop () =
        source.Cancel ()

