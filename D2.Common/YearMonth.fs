namespace D2.Common

open System

[<CustomEquality>]
[<CustomComparison>]
type YearMonth =
    struct
        val private monthNumber : int
        val private yearNumber : int
        
        new (year : int, month : int) =
            { yearNumber = year;  monthNumber = month }
    
        new (dateTime : DateTime) =
            { yearNumber = dateTime.Year;  monthNumber = dateTime.Month }
    end
    
    interface IComparable<YearMonth> with
        member this.CompareTo other =
            (this.yearNumber * 12 + this.monthNumber) - (other.yearNumber * 12 + other.monthNumber)

    interface IComparable with
        member this.CompareTo other =
            match other with 
            | :? YearMonth as yearMonth -> (this.yearNumber * 12 + this.monthNumber)
                                           -
                                           (yearMonth.yearNumber * 12 + yearMonth.monthNumber)
            | _                         -> failwith "cannot compare this with given type"
    
    override this.Equals (other : obj) =
        match other with 
            | :? YearMonth as yearMonth -> this.yearNumber = yearMonth.yearNumber
                                           &&
                                           this.monthNumber = yearMonth.monthNumber
            | _                         -> false
     
    override this.GetHashCode () =
        this.yearNumber.GetHashCode() ^^^ this.yearNumber.GetHashCode()
    
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