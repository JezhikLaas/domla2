namespace D2.Common

open System

[<CustomEquality>]
[<CustomComparison>]
type Date =
    struct
        val Days : int
        
        new (days : int) =
            { Days = days }
    
        new (day : int, month : int, year : int) =
            { Days = (DateTime(year, month, day) - DateTime.MinValue).TotalDays |> int }
    
        new (dateTime : DateTime) =
            { Days = (dateTime - DateTime.MinValue).TotalDays |> int }
    end
    
    interface IComparable<Date> with
        member this.CompareTo other =
            this.Days - other.Days

    interface IComparable with
        member this.CompareTo other =
            match other with 
            | :? Date as date -> this.Days - date.Days
            | _               -> failwith "cannot compare date to given type"

    override this.ToString() =
        let pointInTime = DateTime.MinValue.AddDays (float this.Days)
        sprintf "%04d-%02d-%02d" pointInTime.Year pointInTime.Month pointInTime.Day
    
    override this.Equals (other : obj) =
        match other with 
        | :? Date as date -> date.Days = this.Days
        | _               -> false
     
    override this.GetHashCode () =
        this.Days.GetHashCode ()
    
    member this.Day
        with get () =
             let pointInTime = DateTime.MinValue.AddDays (float this.Days)
             pointInTime.Day
     
    member this.Month
        with get () =
             let pointInTime = DateTime.MinValue.AddDays (float this.Days)
             pointInTime.Month
     
    member this.Year
        with get () =
             let pointInTime = DateTime.MinValue.AddDays (float this.Days)
             pointInTime.Year
    
    static member (+) (left : Date, right : Date) =
        Date (left.Days + right.Days)
    
    static member (-) (left : Date, right : Date) =
        Date (left.Days - right.Days)
