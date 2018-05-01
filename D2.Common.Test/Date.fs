namespace D2CommonTest

open D2.Common
open FsUnit
open NUnit.Framework
open System

[<TestFixture>]
[<Category("Data types")>]
module DateTest =

    type DateRange =
        static member testCases
            with get () =
                let testData = [
                    (2016, 2, 29);
                    (500, 12, 31);
                    (2018, 2, 28);
                    (3000, 1, 1);
                    (1, 1, 1);
                    (9999, 12, 31);
                ]
                seq {
                    for date in testData do
                        let year, month, day = date
                        yield TestCaseData(year, month, day).Returns(sprintf "%04d-%02d-%02d" year month day) 
                }

    [<Test>]
    let ``Format should be YYYY-MM-DD``() =
        let target = Date (1978, 6, 9)
        target.ToString () |> should equal "1978-06-09"

    [<Test>]
    let ``Adding 1001-1-1 to 1000-1-1 results in 2000-1-1``() =
        let target = Date (1000, 1, 1) + Date (1001, 1, 1)  
        target.ToString () |> should equal "2000-01-01"

    [<Test>]
    let ``Subtracting an added value yields the original value``() =
        let left = Date (1000, 1, 1)
        let right = left + Date (1001, 1, 1) - Date (1001, 1, 1)
        right |> should equal left

    [<Test>]
    let ``Sorting works correctly for Dates``() =
        let one = Date (1000, 1, 1)
        let two = Date (1001, 1, 1)
        let three = Date (1002, 1, 1)
        
        let dates = [| three; one; two |] |> Array.sort
        
        dates.[0] |> should equal one
        dates.[1] |> should equal two
        dates.[2] |> should equal three

    [<Test>]
    [<TestCaseSource(typeof<DateRange>, "testCases")>]
    let ``Iterating over a range of dates yields expected values``(year : int, month : int, day : int) =
        Date(year, month, day).ToString()

    [<Test>]
    let ``Converting to DateTime yields instance representing the same date``() =
        let target = Date(1978, 6, 9).DateTime
        target.Year |> should equal 1978
        target.Month |> should equal 6
        target.Day |> should equal 9

    [<Test>]
    let ``Adding months to a date yields the same result as the DateTime calculation``() =
        let target = Date(2016, 2, 29).AddMonths(12)
        let comparison = DateTime(2016, 2, 29).AddMonths(12)
        target.Year |> should equal (comparison.Year)
        target.Month |> should equal (comparison.Month)
        target.Day |> should equal (comparison.Day)

    [<Test>]
    let ``Week days are calculated correctly``() =
        let target = Date(2018, 5, 29)
        (target + 0).DayOfWeek |> should equal DayOfWeek.Sunday
        (target + 1).DayOfWeek |> should equal DayOfWeek.Monday
        (target + 2).DayOfWeek |> should equal DayOfWeek.Tuesday
        (target + 3).DayOfWeek |> should equal DayOfWeek.Wednesday
        (target + 4).DayOfWeek |> should equal DayOfWeek.Thursday
        (target + 5).DayOfWeek |> should equal DayOfWeek.Friday
        (target + 6).DayOfWeek |> should equal DayOfWeek.Saturday

    [<Test>]
    let ``Today for date yields the same result as the DateTime calculation``() =
        let target = Date.Today
        let comparison = DateTime.Today
        target.Year |> should equal (comparison.Year)
        target.Month |> should equal (comparison.Month)
        target.Day |> should equal (comparison.Day)
