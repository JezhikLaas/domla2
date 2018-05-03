namespace D2.Common

open System
open System
open System.Runtime.InteropServices

type YearMonth =
    struct
        val private monthNumber : int
        val private yearNumber : int
        
        new (year : int, month : int) =
            { yearNumber = year;  monthNumber = month }
    
        new (dateTime : DateTime) =
            { yearNumber = dateTime.Year;  monthNumber = dateTime.Month }
    end

    override this.ToString () =
        sprintf "%04d-%02d" this.yearNumber this.monthNumber

    member this.AddMonths (months : int) =
        let total = this.yearNumber * 12 + this.monthNumber + months
        let yearTarget = total / 12
        let monthTarget = total % 12
        match monthTarget with 
        | 0 -> YearMonth (yearTarget - 1, 12)
        | _ -> YearMonth (yearTarget, monthTarget)
    
    member this.Year 
        with get() =
            this.yearNumber
    
    member this.Month
        with get() =
            this.monthNumber