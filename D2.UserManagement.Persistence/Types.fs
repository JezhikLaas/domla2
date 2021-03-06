﻿namespace D2.UserManagement.Persistence

open D2.Common
open Microsoft.Extensions.Logging
open System
open System.Security.Claims

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
    
    abstract member Password : string
    
    abstract member Version : int

    abstract member LoggedIn : DateTime option
    
    abstract member PrivacyAccepted : DateTime option
    
    abstract member UserClaims : Claim list

type StorageService = {
    register : (UserRegistration -> Async<RegistrationResult>)
    listPending : Async<UserRegistration list>
    acceptRegistration : (Guid -> ILogger -> (UserRegistration -> Async<bool>) -> Async<bool>)
    finishRegistration : (Guid -> string -> ILogger -> Async<bool * string>)
    createDatabase : (string -> ILogger -> Async<unit>) 
}


