namespace D2CommonTest

open D2.Common
open FsUnit
open NUnit.Framework
open System

[<TestFixture>]
[<Category("Caching")>]
module CacheTest =
    open System.Threading

    [<Test>]
    let ``Cache can be created``() =
        let options = {
            createItem = fun (x : int) -> x.ToString();
            dropItem = fun (x : string) -> ();
            secondsToLive = 10;
            sweepInterval = 1;
        }
        use cache = new Cache<string, int>(options)
        true |> should equal true

    [<Test>]
    let ``Cache calls create only once when requested two times for the same item within the lifetime span``() =
        let mutable counter = 0
        let options = {
            createItem = fun (x : int) -> counter <- counter + 1; x.ToString();
            dropItem = fun (x : string) -> ();
            secondsToLive = 10;
            sweepInterval = 1;
        }
        use cache = new Cache<string, int>(options)
        cache.fetch 1 |> ignore
        cache.fetch 1 |> ignore
        
        counter |> should equal 1

    [<Test>]
    let ``Cache calls create repeatedly for the same item if requested beyound lifetime span``() =
        let mutable counter = 0
        let waiter = new ManualResetEventSlim(false)
        let options = {
            createItem = fun (x : int) -> counter <- counter + 1; x.ToString();
            dropItem = fun (x : string) -> waiter.Set();
            secondsToLive = 1;
            sweepInterval = 1;
        }
        use cache = new Cache<string, int>(options)
        cache.fetch 1 |> ignore
        let eventTriggered = waiter.Wait(5000)
        cache.fetch 1 |> ignore
        
        eventTriggered |> should equal true
        counter |> should equal 2

    [<Test>]
    let ``Cache invokes dropItem if sweep removes items``() =
        let mutable counter = 0
        let waiter = new ManualResetEventSlim(false)
        let options = {
            createItem = fun (x : int) -> x.ToString();
            dropItem = fun (x : string) -> counter <- counter + 1; waiter.Set();
            secondsToLive = 1;
            sweepInterval = 1;
        }
        use cache = new Cache<string, int>(options)
        cache.fetch 1 |> ignore
        let eventTriggered = waiter.Wait(5000)
        
        eventTriggered |> should equal true
        counter |> should equal 1
