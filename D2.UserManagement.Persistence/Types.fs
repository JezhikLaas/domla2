namespace D2.UserManagement.Persistence

open D2.Common
open System

type RegistrationResult =
    | Ok
    | Known
    | Conflict

type UserRegistration =
    abstract member Id : Guid
    
    abstract member FirstName : string

    abstract member LastName : string

    abstract member Salutation : string

    abstract member Title : string

    abstract member EMail : string

    abstract member Login : string

    abstract member MailSent : DateTime option

type StorageService = {
    register : (UserRegistration -> Async<RegistrationResult>)
    listPending : Async<UserRegistration list>
    markRegistrations : (Guid seq -> Async<unit>)
}


