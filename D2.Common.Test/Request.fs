namespace D2CommonTest

open D2.Common
open FsUnit
open NUnit.Framework
open System

[<TestFixture>]
[<Category("Basics")>]
module RequestHandlerTest =

    [<Test>]
    let ``Check simple external failure``() =
        let failedRequest =  handle { return! failExternal "this failed" }
        failedRequest() |> should equal (ExternalFailure("this failed"))

    [<Test>]
    let ``Check simple success``() =
        let success =  handle { return! succeed "good" }
        success() |> should equal (Success("good"))

    [<Test>]
    let ``Check exception case``() =
        let success =  handle {
            let! c = handle { return 1 / 0 }
            return! succeed c
        }

        match success() with
        | InternalFailure e -> true
        | _ -> false
        
        |> should be True
