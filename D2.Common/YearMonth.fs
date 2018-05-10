namespace D2.Common

open Newtonsoft.Json
open System

type YearMonth =
    struct
        val private monthNumber : int
        val private yearNumber : int
        
        [<JsonConstructor>]
        new (year : int, month : int) =
            { yearNumber = year;  monthNumber = month }
    
        new (dateTime : DateTime) =
            { yearNumber = dateTime.Year;  monthNumber = dateTime.Month }
    end
    
    static member fromObject (instance : obj) =
        match instance with 
        | :? string      as value -> YearMonth(DateTime.Parse(value))
        | :? (int * int) as value -> YearMonth(fst value, snd value)
        | :? DateTime    as value -> YearMonth(value)
        | :? YearMonth   as value -> value
        | _                       -> failwith "could not convert to YearMonth"

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
    
    [<JsonIgnore>]
    member this.DateTime
        with get() =
            DateTime (this.Year, this.Month, 1)