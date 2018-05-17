namespace D2.Common

open Newtonsoft.Json
open System

module TimeStatics =
    
    let calculateSecondsFromMidnight (hour : int) (minute : int) (second : int) =
        if hour < 0 || hour > 23 then failwith (sprintf "invalid hour %d, should be 0 - 23" hour)
        if minute < 0 || minute > 59 then failwith (sprintf "invalid minute %d, should be 0 - 59" minute)
        if second < 0 || second > 59 then failwith (sprintf "invalid second %d, should be 0 - 59" second)
        
        hour * 3600 + minute * 60 + second

[<Serializable>]
type Time =
    struct
        val private secondFromMidnight : int
        
        new (second : int) =
            { secondFromMidnight = second }
        
        [<JsonConstructor>]
        new (hour : int, minute : int, second : int) =
            { secondFromMidnight = TimeStatics.calculateSecondsFromMidnight hour minute second }
    end
    
    [<JsonIgnore>]
    member this.Seconds
        with get () =
             this.secondFromMidnight
    
    member this.Hour
        with get () =
             this.secondFromMidnight / 3600
    
    member this.Minute
        with get () =
             (this.secondFromMidnight % 3600) / 60
    
    member this.Second
        with get () =
             (this.secondFromMidnight % 3600) % 60

    [<JsonIgnore>]
    member this.ShortGermanString =
        sprintf "%02d:%02d" this.Hour this.Minute

    [<JsonIgnore>]
    member this.LongGermanString =
        sprintf "%02d:%02d:%02d" this.Hour this.Minute this.Second