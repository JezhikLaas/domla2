namespace D2CommonTest

open D2.Common
open FsUnit
open Newtonsoft.Json
open NUnit.Framework
open System

[<TestFixture>]
[<Category("Data types")>]
module TimeTest =
    
    [<Test>]
    let ``Constructed time has expected property values``() =
        let target = Time (13, 12, 11)
        target.Hour   |> should equal 13
        target.Minute |> should equal 12
        target.Second |> should equal 11

    [<Test>]
    let ``Instances can be serialized and deserialized``() =
        let source = Time (13, 12, 11)
        let text = JsonConvert.SerializeObject source
        let target = JsonConvert.DeserializeObject<Time> text
        source = target |> should equal true

    [<Test>]
    let ``Invalid hour throws during construction``() =
        (fun () -> new Time (-1, 0, 0) |> ignore) |> should throw typeof<Exception> 
        (fun () -> new Time (24, 0, 0) |> ignore) |> should throw typeof<Exception> 

    [<Test>]
    let ``Invalid minute throws during construction``() =
        (fun () -> new Time (0, -1, 0) |> ignore) |> should throw typeof<Exception> 
        (fun () -> new Time (0, 60, 0) |> ignore) |> should throw typeof<Exception>

    [<Test>]
    let ``Invalid second throws during construction``() =
        (fun () -> new Time (0, 0, -1) |> ignore) |> should throw typeof<Exception> 
        (fun () -> new Time (0, 0, 60) |> ignore) |> should throw typeof<Exception>

    [<Test>]
    let ``Short german string yields expected result``() =
        let target = Time (9, 7, 0)
        target.ShortGermanString |> should equal "09:07"

    [<Test>]
    let ``Long german string yields expected result``() =
        let target = Time (9, 7, 3)
        target.LongGermanString |> should equal "09:07:03"