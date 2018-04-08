namespace D2.UserManagement.Persistence

open D2.Common
open System

type RegistrationResult =
    | Ok
    | Known
    | Conflict

[<AllowNullLiteral>]
type UserRegistration =
    abstract member Id : Guid
    
    abstract member FirstName : string

    abstract member LastName : string

    abstract member Salutation : string

    abstract member Title : string

    abstract member EMail : string

    abstract member Login : string
    
    abstract member Version : int

    abstract member MailSent : DateTime option

[<AllowNullLiteral>]
type User =
    abstract member Id : Guid
    
    abstract member FirstName : string

    abstract member LastName : string

    abstract member Salutation : string

    abstract member Title : string

    abstract member EMail : string

    abstract member Login : string
    
    abstract member Version : int

    abstract member LoggedIn : DateTime option
    
    abstract member PrivacyAccepted : DateTime option

type StorageService = {
    register : (UserRegistration -> Async<RegistrationResult>)
    listPending : Async<UserRegistration list>
    acceptRegistration : (Guid -> (UserRegistration -> Async<bool>) -> Async<bool>)
}


