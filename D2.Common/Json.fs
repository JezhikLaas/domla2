namespace D2.Common

open System

module Json =

    open Newtonsoft.Json

    let serialize (instance : obj) (options : JsonSerializerSettings) =
        JsonField (JsonConvert.SerializeObject (instance, options))

    let deserialize<'T> (data : string) (options : JsonSerializerSettings) =
        JsonConvert.DeserializeObject<'T> (data, options)
