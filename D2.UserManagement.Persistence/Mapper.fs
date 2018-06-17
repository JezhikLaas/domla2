namespace D2.UserManagement.Persistence

open BCrypt.Net
open BCrypt.Net
open Cast
open Newtonsoft.Json
open System
open System.Security.Claims

[<AllowNullLiteral>]
type UserRegistrationI() =
    let mutable id = Guid.Empty
    let mutable firstName = String.Empty
    let mutable lastName = String.Empty
    let mutable salutation = String.Empty
    let mutable title = String.Empty
    let mutable email = String.Empty
    let mutable login = String.Empty
    let mutable mailSent : Nullable<DateTime> = Nullable()
    let mutable version = 0
    
    abstract member Id : Guid with get, set
    default this.Id with get() = id and set(value) = id <- value
    
    abstract member FirstName : string with get, set
    default this.FirstName with get() = firstName and set(value) = firstName <- value

    abstract member LastName : string with get, set
    default this.LastName with get() = lastName and set(value) = lastName <- value

    abstract member Salutation : string with get, set
    default this.Salutation with get() = salutation and set(value) = salutation <- value

    abstract member Title : string with get, set
    default this.Title with get() = title and set(value) = title <- value

    abstract member EMail : string with get, set
    default this.EMail with get() = email and set(value) = email <- value

    abstract member Login : string with get, set
    default this.Login with get() = login
                            and
                            set(value) =
                                login <- if not(value |> isNull) then value.ToLower() else null

    abstract member MailSent : Nullable<DateTime> with get, set
    default this.MailSent with get() = mailSent and set(value) = mailSent <- value

    abstract member Version : int with get, set
    default this.Version with get() = version and set(value) = version <- value
    
    interface UserRegistration with
        member this.Id with get() = this.Id
        member this.FirstName with get() = this.FirstName
        member this.LastName with get() = this.LastName
        member this.Salutation with get() = this.Salutation
        member this.Title with get() = this.Title
        member this.EMail with get() = this.EMail
        member this.Login with get() = this.Login
        member this.MailSent with get() = (if this.MailSent.HasValue then Some this.MailSent.Value else None)
        member this.Version with get() = this.Version


[<AllowNullLiteral>]
type UserI() =
    let mutable id = Guid.Empty
    let mutable firstName = String.Empty
    let mutable lastName = String.Empty
    let mutable salutation = String.Empty
    let mutable title = String.Empty
    let mutable email = String.Empty
    let mutable login = String.Empty
    let mutable password = String.Empty
    let mutable claims = String.Empty
    let mutable userClaims = List.empty<Claim>
    let mutable loggedIn : Nullable<DateTime> = Nullable()
    let mutable privacyAccepted : Nullable<DateTime> = Nullable()
    let mutable version = 0
    
    abstract member Id : Guid with get, set
    default this.Id with get() = id and set(value) = id <- value
    
    abstract member FirstName : string with get, set
    default this.FirstName with get() = firstName and set(value) = firstName <- value

    abstract member LastName : string with get, set
    default this.LastName with get() = lastName and set(value) = lastName <- value

    abstract member Salutation : string with get, set
    default this.Salutation with get() = salutation and set(value) = salutation <- value

    abstract member Title : string with get, set
    default this.Title with get() = title and set(value) = title <- value

    abstract member EMail : string with get, set
    default this.EMail with get() = email and set(value) = email <- value

    abstract member UserClaims : Claim list with get, set
    default this.UserClaims with get() = userClaims and set(value) = userClaims <- value

    abstract member Claims : string with get, set
    default this.Claims
        with get() =
            JsonConvert.SerializeObject(userClaims)
        and set(value) =
            userClaims <- JsonConvert.DeserializeObject<System.Collections.Generic.List<Claim>>(value)
                          |> Seq.toList

    abstract member Login : string with get, set
    default this.Login with get() = login
                            and
                            set(value) =
                                login <- if not(value |> isNull) then value.ToLower() else null

    abstract member Password : string with get, set
    default this.Password with get() = password and set(value) = password <- value

    abstract member LoggedIn : Nullable<DateTime> with get, set
    default this.LoggedIn with get() = loggedIn and set(value) = loggedIn <- value

    abstract member PrivacyAccepted : Nullable<DateTime> with get, set
    default this.PrivacyAccepted with get() = privacyAccepted and set(value) = privacyAccepted <- value

    abstract member Version : int with get, set
    default this.Version with get() = version and set(value) = version <- value
    
    static member fromRegistration (data : UserRegistrationI) (password : string) =
        let salt = BCrypt.GenerateSalt ()
        let hashedPassword = BCrypt.HashPassword (password, salt)
        
        UserI (
            FirstName = data.FirstName,
            LastName = data.LastName,
            Salutation = data.Salutation,
            Title = data.Title,
            EMail = data.EMail,
            Login = data.Login,
            PrivacyAccepted = Nullable(DateTime.UtcNow),
            Password = hashedPassword,
            UserClaims = [
                Claim("role", "user");
                Claim("id", Guid.NewGuid().ToString("N"));
                Claim("name", data.Login);
            ]
        )
    
    interface User with
        member this.Id with get() = this.Id
        member this.FirstName with get() = this.FirstName
        member this.LastName with get() = this.LastName
        member this.Salutation with get() = this.Salutation
        member this.Title with get() = this.Title
        member this.EMail with get() = this.EMail
        member this.Login with get() = this.Login
        member this.Password with get() = this.Password
        member this.LoggedIn with get() = (if this.LoggedIn.HasValue then Some this.LoggedIn.Value else None)
        member this.PrivacyAccepted with get() = (if this.PrivacyAccepted.HasValue then Some this.PrivacyAccepted.Value else None)
        member this.Version with get() = this.Version
        member this.UserClaims with get() = this.UserClaims
