namespace D2.Common

open System
open System
open System.Collections.Generic

type CacheOptions<'T, 'K> = {
    createItem : 'K -> 'T
    dropItem : 'K -> 'T -> unit
    secondsToLive : int
    sweepInterval : int
}

type CacheBucket<'T> (value : 'T) =
    member val item : 'T = value with get, set
    member val access = DateTime.UtcNow with get, set

type Cache<'T, 'K when 'K : equality>(options : CacheOptions<'T, 'K>) =

    let tokenSource = new System.Threading.CancellationTokenSource ()
    let buckets = new Dictionary<'K, CacheBucket<'T>>()
    
    let clearOutdated () =
        lock buckets (
            fun () -> 
                let removals = new List<'K>()
                for bucket in buckets do
                    let unused = (DateTime.UtcNow - bucket.Value.access).TotalSeconds
                    if unused > (float options.secondsToLive) then
                        options.dropItem bucket.Key bucket.Value.item
                        removals.Add bucket.Key
                for key in removals do
                    buckets.Remove key |> ignore
                        
        )
    
    let rec clearLoop () =
        async {
            do! Async.Sleep options.sweepInterval
            clearOutdated ()
            return! clearLoop ()
        }
    
    do
        Async.Start (clearLoop (), tokenSource.Token)
    
    interface IDisposable with
        member this.Dispose() =
            tokenSource.Cancel()
    
    member this.fetch (key : 'K) =
        lock buckets (
            fun () ->
                match buckets.TryGetValue key with 
                | true, value -> value.access <- DateTime.UtcNow
                                 value.item
                | false, _    -> let result = CacheBucket (options.createItem key)
                                 buckets.Add (key, result)
                                 result.item
        )
