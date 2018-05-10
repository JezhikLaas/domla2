namespace D2CommonTest

open D2.Common
open FsUnit
open NUnit.Framework
open System

[<TestFixture>]
[<Category("Data types")>]
module YearMonthTest =
    open Newtonsoft.Json

    [<Test>]
    let ``Format should be YYYY-MM``() =
        let target = YearMonth (978, 6)
        target.ToString () |> should equal "0978-06"

    [<Test>]
    let ``Adding months yield the expected results``() =
        let target = YearMonth (1978, 6)
        target.AddMonths(12).Year |> should equal 1979
        target.AddMonths(12).Month |> should equal 6

    [<Test>]
    let ``Sorting works correctly for YearMonths``() =
        let one = YearMonth (1000, 1)
        let two = YearMonth (1001, 1)
        let three = YearMonth (1002, 1)
        
        let dates = [| three; one; two |] |> Array.sort
        
        dates.[0] |> should equal one
        dates.[1] |> should equal two
        dates.[2] |> should equal three

    [<Test>]
    let ``Equal sign yields true for equal values``() =
        let left = YearMonth (1978, 6)
        let right = YearMonth (1978, 6)
        left = right |> should equal true

    [<Test>]
    let ``Equal sign yields false for values which are not equal``() =
        let left = YearMonth (1978, 5)
        let right = YearMonth (1978, 6)
        left = right |> should equal false

    [<Test>]
    let ``Inequal test yields true for values which are not equal``() =
        let left = YearMonth (1978, 5)
        let right = YearMonth (1978, 6)
        left <> right |> should equal true

    [<Test>]
    let ``Inequal test yields false for values which are equal``() =
        let left = YearMonth (1978, 6)
        let right = YearMonth (1978, 6)
        left <> right |> should equal false

    [<Test>]
    let ``Instances can be serialized and deserialized``() =
        let source = YearMonth (1978, 6)
        let text = JsonConvert.SerializeObject source
        let target = JsonConvert.DeserializeObject<YearMonth> text
        source = target |> should equal true
