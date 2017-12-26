namespace D2.Authentication

open System.Threading

type TokenCleanup (storage : PersistedGrantStorage) =

    let source = new CancellationTokenSource()

    let clearOutdatedTokens () =
        storage.removeOutdated ()
        |> Async.RunSynchronously
        ()

    let run =
        async {
            while true do
                do! Async.Sleep(5000)
                clearOutdatedTokens ()
        }

    member this.Start () =
        Async.Start (run, source.Token)
    
    member this.Stop () =
        source.Cancel ()

