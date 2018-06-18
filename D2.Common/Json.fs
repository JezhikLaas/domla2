namespace D2.Common

module Json =

    open Newtonsoft.Json
    open Newtonsoft.Json.Linq
    open System
    open System.Collections.Generic
    open System.Security.Claims
    
    type ClaimConverter() =
        inherit JsonConverter()
        
        override this.CanConvert(objectType : Type) =
            objectType = typeof<Claim>
        
        override this.CanWrite = false
    
        override this.WriteJson(writer : JsonWriter, value : obj, serializer : JsonSerializer) =
            ()
        
        override this.ReadJson(reader : JsonReader, objectType : Type, existingValue : obj, serializer : JsonSerializer) =
            let raw = JObject.Load(reader)
            let typeValue = string raw.["Type"]
            let issuer = string raw.["Issuer"]
            let originalIssuer = string raw.["OriginalIssuer"]
            let value = string raw.["Value"]
            let valueType = string raw.["ValueType"]
    
            if raw.["Subject"] <> null && raw.["Subject"].HasValues then
                let inner = raw.["Subject"].CreateReader()
                let identity = serializer.Deserialize<ClaimsIdentity>(inner)
                new Claim(typeValue, value, valueType, issuer, originalIssuer, identity) :> obj
            else
                new Claim(typeValue, value, valueType, issuer, originalIssuer) :> obj

    type ClaimsIdentityConverter() =
        inherit JsonConverter()
        
        override this.CanConvert(objectType : Type) =
            objectType = typeof<ClaimsIdentity>
        
        override this.CanWrite = false
    
        override this.WriteJson(writer : JsonWriter, value : obj, serializer : JsonSerializer) =
            ()
        
        override this.ReadJson(reader : JsonReader, objectType : Type, existingValue : obj, serializer : JsonSerializer) =
            let raw = JObject.Load(reader)
            let actor = raw.["Actor"].ToObject<ClaimsIdentity>()
            let authenticationType = string raw.["AuthenticationType"]
            let bootstrapContext = string raw.["BootstrapContext"]
            let label = string raw.["Label"]
            let nameClaimType = string raw.["NameClaimType"]
            let roleClaimType = string raw.["RoleClaimType"]
            let claimsReader = raw.["Claims"].CreateReader()
            let claims = serializer.Deserialize<IEnumerable<Claim>>(claimsReader)
            
            let result = new ClaimsIdentity(
                             actor,
                             claims,
                             authenticationType,
                             nameClaimType,
                             roleClaimType
                         )
            result.BootstrapContext <- bootstrapContext
            result.Label <- label
            result :> obj
    
    let jsonOptions = 
        let result = new JsonSerializerSettings()
        result.Converters.Add(new ClaimConverter())
        result.Converters.Add(new ClaimsIdentityConverter())
        result.ReferenceLoopHandling <- ReferenceLoopHandling.Ignore
        result
    
    let serialize (instance : obj) = 
        JsonConvert.SerializeObject (instance, jsonOptions)
    
    let deserialize<'T> (data : string) = 
        JsonConvert.DeserializeObject<'T> (data, jsonOptions)
        
    type Converter (options : JsonSerializerSettings) =
        member this.serialize (instance : obj) =
            JsonField (JsonConvert.SerializeObject (instance, options))
        
        member this.deserialize<'T> (data : string) =
            JsonConvert.DeserializeObject<'T> (data, options)
