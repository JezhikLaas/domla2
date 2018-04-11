namespace D2.UserManagement.Persistence

open D2.Common
open Microsoft.Extensions.Logging
open System
open System.Collections.Generic

module Users =
    open NHibernate
    open NHibernate.Criterion
    open NHibernate.Linq
    open System.Linq
    
    let private isAccepted (user : UserRegistration) =
        async {
            use session = Connection.readOnlySession ()
            use transaction = session.BeginTransaction()

            let email = user.EMail.ToLowerInvariant()
            let login = user.Login.ToLowerInvariant()

            let! existing = session
                                .Query<UserI>()
                                .Where(fun reg -> reg.EMail = email || reg.Login = login)
                                .SingleOrDefaultAsync()
                                |> Async.AwaitTask
            
            do! transaction.CommitAsync() |> Async.AwaitTask
            
            match existing = null with
            | true  -> return RegistrationResult.Ok
            | false -> if existing.EMail = email then
                           return RegistrationResult.Known
                        else
                           return RegistrationResult.Conflict
        }
        
    let private registerUserWorker (user : UserRegistration) =
        let knownRegistration (session : ISession) =
            async {
                let email = user.EMail.ToLowerInvariant()
                let login = user.Login.ToLowerInvariant()

                let! existing = session
                                    .Query<UserRegistrationI>()
                                    .Where(fun reg -> reg.EMail = email || reg.Login = login)
                                    .SingleOrDefaultAsync()
                                    |> Async.AwaitTask

                match existing = null with
                | false -> return Some (existing.Login, existing.EMail)
                | true  -> return None
            }

        async {
            use session = Connection.readOnlySession ()
            use transaction = session.BeginTransaction()

            let! known = knownRegistration session

            match known with
            | None   -> let! accepted = isAccepted user 
                        match accepted with
                        | RegistrationResult.Ok -> session.Save user |> ignore
                                                   transaction.Commit ()
                                                   return RegistrationResult.Ok
                        
                        | result                -> return result
            
            | Some u -> if snd u = user.EMail.ToLowerInvariant() then
                            return RegistrationResult.Known
                        else
                            return RegistrationResult.Conflict
        }
    
    let private listPendingUsersWorker = 
        async {
            use session = Connection.readOnlySession ()
            use transaction = session.BeginTransaction()
            let! pendings = session
                             .Query<UserRegistrationI>()
                             .Where(fun reg -> reg.MailSent = Nullable())
                             .ToListAsync()
                             |> Async.AwaitTask
            transaction.Commit()
            return pendings |> Seq.map(fun reg -> reg :> UserRegistration) |> Seq.toList
        }
    
    let private acceptRegistrationWorker (id : Guid) (logger : ILogger) (prerequisite : (UserRegistration -> Async<bool>)) =
        async {
            use session = Connection.session ()
            use transaction = session.BeginTransaction()
            
            let! registration = session.GetAsync<UserRegistrationI>(id) |> Async.AwaitTask
            if registration |> isNull then
                return false
            else
                let! canContinue = prerequisite (registration) 
                if canContinue then
                    try
                        registration.MailSent <- Nullable<DateTime>(DateTime.UtcNow)
                        session.Update(registration)
                        do! transaction.CommitAsync () |> Async.AwaitTask
                        return true
                    with
                    | error -> logger.LogError (error, "acceptRegistrationWorker failed")
                               return false
                else
                    return canContinue
        }
    
    let Storage = {
        register = registerUserWorker;
        listPending = listPendingUsersWorker;
        acceptRegistration = acceptRegistrationWorker;
    }