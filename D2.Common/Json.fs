namespace D2.Common

module Json =

    open Newtonsoft.Json

    type Converter (options : JsonSerializerSettings) =
        member this.serialize (instance : obj) =
            JsonField (JsonConvert.SerializeObject (instance, options))
        
        member this.deserialize<'T> (data : string) =
            JsonConvert.DeserializeObject<'T> (data, options)
