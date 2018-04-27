namespace D2CommonTest

open D2.Common
open FsUnit
open NUnit.Framework
open System

[<TestFixture>]
[<Category("Data types")>]
module DateTest =

    [<Test>]
    let ``Format should be YYYY-MM-DD``() =
        let target = Date (9, 6, 1978)
        target.ToString () |> should equal "1978-06-09"

    [<Test>]
    let ``Adding 1001-1-1 to 1000-1-1 results in 2000-1-1``() =
        let target = Date (1, 1, 1000) + Date (1, 1, 1001)  
        target.ToString () |> should equal "2000-01-01"

    [<Test>]
    let ``Subtracting an added value yields the original value``() =
        let left = Date (1, 1, 1000)
        let right = left + Date (1, 1, 1001) - Date (1, 1, 1001)
        right |> should equal left

    [<Test>]
    let ``Sorting works correctly for Dates``() =
        let one = Date (1, 1, 1000)
        let two = Date (1, 1, 1001)
        let three = Date (1, 1, 1002)
        
        let dates = [| three; one; two |] |> Array.sort
        
        dates.[0] |> should equal one
        dates.[1] |> should equal two
        dates.[2] |> should equal three
