namespace D2CommonTest

open D2.Common
open FsUnit
open NUnit.Framework
open System

[<TestFixture>]
[<Category("Data types")>]
module YearMonthTest =

    [<Test>]
    let ``Format should be YYYY-MM``() =
        let target = YearMonth (978, 6)
        target.ToString () |> should equal "0978-06"

    [<Test>]
    let ``Adding months yield the expected results``() =
        let target = YearMonth (1978, 6)
        target.AddMonths(12).Year |> should equal 1979
        target.AddMonths(12).Month |> should equal 6