namespace D2.Authentication

open D2.Common
open IdentityServer4.Models
open Newtonsoft.Json
open Newtonsoft.Json.Linq
open System
open System.Collections.Generic
open System.Data.Common
open System.Security.Claims
open System.Threading.Tasks

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

type RefreshTokenConverter() =
    inherit JsonConverter()
    
    override this.CanConvert(objectType : Type) =
        objectType = typeof<RefreshToken>
    
    override this.CanWrite = false

    override this.WriteJson(writer : JsonWriter, value : obj, serializer : JsonSerializer) =
        ()
    
    override this.ReadJson(reader : JsonReader, objectType : Type, existingValue : obj, serializer : JsonSerializer) =
        let raw = JObject.Load(reader)
        
        let accesTokenReader = raw.["AccessToken"].CreateReader()
        let accessToken = serializer.Deserialize<Token>(accesTokenReader)
        
        let subjectReader = raw.["Subject"].CreateReader()
        let subject = serializer.Deserialize<ClaimsPrincipal>(subjectReader)
        
        let creationTime = raw.["CreationTime"].ToObject<DateTime>()
        let lifeTime = raw.["LifeTime"].ToObject<int>()
        let version = raw.["Version"].ToObject<int>()
        
        let token = new RefreshToken(
                            AccessToken = accessToken,
                            CreationTime = creationTime,
                            Lifetime = lifeTime,
                            Version = version
                        )
        token :> obj

type ClaimsPrincipalConverter() =
    inherit JsonConverter()
    
    override this.CanConvert(objectType : Type) =
        objectType = typeof<ClaimsPrincipal>
    
    override this.CanWrite = false

    override this.WriteJson(writer : JsonWriter, value : obj, serializer : JsonSerializer) =
        ()
    
    override this.ReadJson(reader : JsonReader, objectType : Type, existingValue : obj, serializer : JsonSerializer) =
        let raw = JObject.Load(reader)
        
        if raw.["Identity"] <> null && raw.["Identity"].HasValues then
            let identityReader = raw.["Identity"].CreateReader()
            let identity = serializer.Deserialize<ClaimsIdentity>(identityReader)
            new ClaimsPrincipal(identity) :> obj
        else 
            let identityReader = raw.["Identities"].CreateReader()
            let identities = serializer.Deserialize<IEnumerable<ClaimsIdentity>>(identityReader)
            new ClaimsPrincipal(identities) :> obj

module Json =
    let jsonOptions = 
        let result = new JsonSerializerSettings()
        result.Converters.Add(new ClaimConverter())
        result.Converters.Add(new ClaimsIdentityConverter())
        result.Converters.Add(new ClaimsPrincipalConverter())
        result.Converters.Add(new RefreshTokenConverter())
        result.ReferenceLoopHandling <- ReferenceLoopHandling.Ignore
        result

type User  =
    abstract member Id : Guid
    abstract member Login : String
    abstract member FirstName : String
    abstract member LastName : String
    abstract member EMail : String
    abstract member Password : String
    abstract member Title : String
    abstract member Salutation : String
    abstract member Claims : Claim list
    abstract member LoggedIn : bool

type UserI () =
    member val Id = Guid.Empty with get, set
    member val Login = String.Empty with get, set
    member val FirstName = String.Empty with get, set
    member val LastName = String.Empty with get, set
    member val EMail = String.Empty with get, set
    member val Password = String.Empty with get, set
    member val Title = String.Empty with get, set
    member val Salutation = String.Empty with get, set
    member val Claims = List.empty<Claim> with get, set
    member val LoggedIn = false with get, set
    interface User with
        member this.Id with get() = this.Id
        member this.Login with get() = this.Login
        member this.FirstName with get() = this.FirstName
        member this.LastName with get() = this.LastName
        member this.EMail with get() = this.EMail
        member this.Password with get() = this.Password
        member this.Title with get() = this.Title
        member this.Salutation with get() = this.Salutation
        member this.Claims with get() = this.Claims
        member this.LoggedIn with get() = this.LoggedIn
    
    static member fromReader (reader : DbDataReader) =
        let jsonIndex = reader.GetOrdinal("claims")
        let idIndex = reader.GetOrdinal("id")
        let loginIndex = reader.GetOrdinal("login")
        let firstNameIndex = reader.GetOrdinal("first_name")
        let lastNameIndex = reader.GetOrdinal("last_name")
        let emailIndex = reader.GetOrdinal("email")
        let titleIndex = reader.GetOrdinal("title")
        let salutationIndex = reader.GetOrdinal("salutation")
        let loggedInIndex = reader.GetOrdinal("logged_in")
        new UserI(
            Id = reader.GetGuid(idIndex),
            Login = reader.GetString(loginIndex),
            FirstName = (if reader.IsDBNull(firstNameIndex) then String.Empty else reader.GetString(firstNameIndex)),
            LastName = reader.GetString(lastNameIndex),
            EMail = reader.GetString(emailIndex),
            Title = (if reader.IsDBNull(titleIndex) then String.Empty else reader.GetString(titleIndex)),
            Salutation = (if reader.IsDBNull(salutationIndex) then String.Empty else reader.GetString(salutationIndex)),
            Claims = (if reader.IsDBNull(jsonIndex) then List.empty<Claim> else (JsonConvert.DeserializeObject<Claim list>(reader.GetString(jsonIndex), Json.jsonOptions))),
            LoggedIn = (reader.IsDBNull(loggedInIndex) = false)
        ) :> User

type AuthorizationStorage = {
    storeAuthorizationCode : AuthorizationCode -> Async<string>
    getAuthorizationCode : string -> Async<AuthorizationCode option>
    removeAuthorizationCode : string -> Async<unit>
}

type ConnectionOptions = {
    Database : string
    Host : string
    User : string
    Password : string
    Port : int
}

type SetupStorage = {
    initialize : unit -> Async<unit>
}

type PersistedGrantStorage = {
    getAll : string -> Async<PersistedGrant seq>
    get : string-> Async<PersistedGrant option>
    removeAll : string -> string -> Async<unit>
    removeAllType : string -> string -> string -> Async<unit>
    remove : string -> Async<unit>
    removeOutdated : unit -> Async<unit>
    store : PersistedGrant -> Async<unit>
}

type ResourceStorage = {
    findApiResource : string -> Async<ApiResource option>
    findApiResourcesByScope : string seq -> Async<ApiResource seq>
    findIdentityResourcesByScope : string seq -> Async<IdentityResource seq>
    getAllResources : unit -> Async<Resources>
}

type ClientStorage = {
    findClientById : string -> Async<Client option>
}

type UserStorage = {
    findUser : string -> string -> Async<User option>
    fetchUser : string -> Async<User option>
    updateActive : string -> bool -> Async<unit>
}

type Storages = {
    persistedGrantStorage : ConnectionOptions -> PersistedGrantStorage
    resourceStorage : ConnectionOptions -> ResourceStorage
    clientStorage : ConnectionOptions -> ClientStorage
    userStorage : ConnectionOptions -> UserStorage
    authorizationStorage : ConnectionOptions -> AuthorizationStorage
}