namespace D2.UserManagement.Persistence

open Cast
open System

module Mapper =

    type UserRegistrationI() =
        let mutable login = String.Empty
        member val Id = Guid.Empty with get, set
        member val FirstName = String.Empty with get, set
        member val LastName = String.Empty with get, set
        member val Salutation = String.Empty with get, set
        member val Title = String.Empty with get, set
        member val EMail = String.Empty with get, set
        member val MailSent = None with get, set
        member this.Login
            with get() = login
            and set(value : String) =
                login <- if not(value |> isNull) then value.ToLower() else null
        
        interface UserRegistration with
            member this.Id with get() = this.Id
            member this.FirstName with get() = this.FirstName
            member this.LastName with get() = this.LastName
            member this.Salutation with get() = this.Salutation
            member this.Title with get() = this.Title
            member this.EMail with get() = this.EMail
            member this.Login with get() = this.Login
            member this.MailSent with get() = this.MailSent
