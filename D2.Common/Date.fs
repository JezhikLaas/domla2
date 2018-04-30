namespace D2.Common

open System

module private DateStatics =
    
    let ticksPerMillisecond = 10000L
    let ticksPerSecond = ticksPerMillisecond * 1000L
    let ticksPerMinute = ticksPerSecond * 60L
    let ticksPerHour = ticksPerMinute * 60L
    let ticksPerDay = ticksPerHour * 24L
    let daysPerYear = 365
    let daysPer4Years = daysPerYear * 4 + 1
    let daysPer100Years = daysPer4Years * 25 - 1
    let daysPer400Years = daysPer100Years * 4 + 1
    let datePartYear = 0
    let datePartDayOfYear = 1
    let datePartMonth = 2
    let datePartDay = 3
    
    let daysToMonth365 = [|
        0; 31; 59; 90; 120; 151; 181; 212; 243; 273; 304; 334; 365
    |]
    
    let daysToMonth366 = [|
        0; 31; 60; 91; 121; 152; 182; 213; 244; 274; 305; 335; 366
    |]

    let isLeapYear (year : int) =
        if year < 1 || year > 9999 then raise (ArgumentOutOfRangeException "year")
        year % 4 = 0 && (year % 100 <> 0 || year % 400 = 0)

    let isValidDate (year : int)(month : int)(day : int) =
        if year < 1 || year > 9999 || month < 1 || month > 12 then
            false
        else
            let days = if isLeapYear(year) then daysToMonth366 else daysToMonth365
            if day < 1 || day > days.[month] - days.[month - 1] then
                false
            else
                true
    
    let dateToDayNumber (year : int)(month : int)(day : int) =
        if year < 1 || year > 9999 || month < 1 || month > 12 then
            raise (ArgumentOutOfRangeException(sprintf "%4d-%2d-%2d is not a valid date" year month day))
        
        let days = if isLeapYear(year) then daysToMonth366 else daysToMonth365
        if day < 1 || day > days.[month] - days.[month - 1] then
            raise (ArgumentOutOfRangeException(sprintf "%4d-%2d-%2d is not a valid date" year month day))
        
        let y = year - 1
        y * 365 + y / 4 - y / 100 + y / 400 + days.[month - 1] + day - 1
    
    let daysInMonth (year : int)(month : int) =
        if month < 1 || month > 12 then raise (ArgumentOutOfRangeException(sprintf "%d is not a valid month" month))
        let days = if isLeapYear(year) then daysToMonth366 else daysToMonth365
        days.[month] - days.[month - 1]

[<CustomEquality>]
[<CustomComparison>]
type Date =
    struct
        val dayNumber : int
        
        new (days : int) =
            { dayNumber = days }
    
        new (year : int, month : int, day : int) =
            { dayNumber = DateStatics.dateToDayNumber year month day }
    
        new (dateTime : DateTime) =
            { dayNumber = DateStatics.dateToDayNumber dateTime.Year dateTime.Month dateTime.Day }
    end
    
    member private this.getDatePart (part: int) =
        let mutable n = this.dayNumber
        let y400 = n / DateStatics.daysPer400Years
        n <- n - y400 * DateStatics.daysPer400Years
        let mutable y100 = n / DateStatics.daysPer100Years
        if y100 = 4 then y100 <- 3
        n <- n - y100 * DateStatics.daysPer100Years
        let y4 = n / DateStatics.daysPer4Years
        n <- n - y4 * DateStatics.daysPer4Years
        let mutable y1 = n / DateStatics.daysPerYear
        if y1 = 4 then y1 <- 3
        if part = DateStatics.datePartYear then
            y400 * 400 + y100 * 100 + y4 * 4 + y1 + 1
        else
            n <- n - y1 * DateStatics.daysPerYear
            if part = DateStatics.datePartDayOfYear then
                n + 1
            else
                let leapYear = y1 = 3 && (y4 <> 24 || y100 = 3)
                let days = if leapYear then DateStatics.daysToMonth366 else DateStatics.daysToMonth365
                let mutable m = (n >>> 5) + 1
                while n >= days.[m] do m <- m + 1
                if part = DateStatics.datePartMonth then
                    m
                else
                    n - days.[m - 1] + 1
    
    member private this.getDatePart () =
        let mutable n = this.dayNumber
        let y400 = n / DateStatics.daysPer400Years
        n <- n - y400 * DateStatics.daysPer400Years
        let mutable y100 = n / DateStatics.daysPer100Years
        if y100 = 4 then y100 <- 3
        n <- n - y100 * DateStatics.daysPer100Years
        let y4 = n / DateStatics.daysPer4Years
        n <- n - y4 * DateStatics.daysPer4Years
        let mutable y1 = n / DateStatics.daysPerYear
        if y1 = 4 then y1 <- 3
        n <- n - y1 * DateStatics.daysPerYear
        let leapYear = y1 = 3 && (y4 <> 24 || y100 = 3)
        let days = if leapYear then DateStatics.daysToMonth366 else DateStatics.daysToMonth365
        let mutable m = (n >>> 5) + 1
        while n >= days.[m] do m <- m + 1
        (y400 * 400 + y100 * 100 + y4 * 4 + y1 + 1, m, n - days.[m - 1] + 1)
    
    interface IComparable<Date> with
        member this.CompareTo other =
            this.dayNumber - other.dayNumber

    interface IComparable with
        member this.CompareTo other =
            match other with 
            | :? Date as date -> this.dayNumber - date.dayNumber
            | _               -> failwith "cannot compare date to given type"

    override this.ToString() =
        let year, month, day = this.getDatePart ()
        sprintf "%04d-%02d-%02d" year month day
    
    override this.Equals (other : obj) =
        match other with 
        | :? Date as date -> date.dayNumber = this.dayNumber
        | _               -> false
     
    override this.GetHashCode () =
        this.dayNumber.GetHashCode ()
    
    member this.Day
        with get () =
             this.getDatePart DateStatics.datePartDay
     
    member this.Month
        with get () =
             this.getDatePart DateStatics.datePartMonth
     
    member this.Year
        with get () =
             this.getDatePart DateStatics.datePartYear
     
    member this.DateTime
        with get () =
             DateTime(int64 this.dayNumber * DateStatics.ticksPerDay)
    
    member this.DayOfWeek
        with get () =
            enum<DayOfWeek> ((this.dayNumber - 1) % 7)
                
    static member (+) (left : Date, right : Date) =
        Date (left.dayNumber + right.dayNumber)
    
    static member (+) (left : Date, right : int) =
        Date (left.dayNumber + right)
    
    static member (-) (left : Date, right : Date) =
        Date (left.dayNumber - right.dayNumber)
    
    static member (-) (left : Date, right : int) =
        Date (left.dayNumber - right)
    
    member this.AddMonths (months : int) =
        let mutable year, month, day = this.getDatePart()
        let i = month - 1 + months
        if i >= 0 then
            month <- i % 12 + 1
            year  <- year + i / 12
        else
            month <- 12 + (i + 1) % 12
            year  <- year + (i - 11) / 12

        if year < 1 || year > 9999 then
            raise(ArgumentOutOfRangeException(sprintf "calculation results in an invalid year (%d)" year))
        
        let days = DateStatics.daysInMonth year month
        if day > days then day <- days
        Date(year, month, day)

    member this.AddYears (years : int) =
        this.AddMonths (12 * years)
