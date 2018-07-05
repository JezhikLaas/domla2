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
                use transaction = session.BeginTransaction()
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
            use session = Connection.session ()
            let! known = knownRegistration session

            match known with
            | None   -> let! accepted = isAccepted user 
                        match accepted with
                        | RegistrationResult.Ok -> use transaction = session.BeginTransaction() 
                                                   session.Save (UserRegistrationI.fromRegistration user) |> ignore
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
    
    let private finishRegistrationWorker (id : Guid) (password : string) (logger : ILogger) =
        async {
            use session = Connection.session ()
            use transaction = session.BeginTransaction()
            
            let! registration = session.GetAsync<UserRegistrationI>(id) |> Async.AwaitTask
            if registration |> isNull then
                logger.LogWarning (sprintf "no registration with if %s found" (id.ToString()))
                return (false, String.Empty)
            else
                let user = UserI.fromRegistration registration password
                session.Delete registration
                session.Save user |> ignore
                do! transaction.CommitAsync () |> Async.AwaitTask
                logger.LogInformation (sprintf "user with login %s successfully activated" user.Login)
                
                let dbkey = user.UserClaims
                            |> List.find(fun u -> u.Type = "dbkey")
                
                return (true, dbkey.Value)
        }
    
    let private createUserDatabaseWorker (dbkey : string) =
        async {
            use connection = AdminConnection.connection ()
            use command = connection.CreateCommand()
            
            command.CommandText <- "CREATE ROLE " + dbkey + " LOGIN PASSWORD '" + dbkey + "'"
            command.ExecuteNonQuery () |> ignore
            
            command.CommandText <- "GRANT " + dbkey + " TO " + AdminConnection.fetchAdminRole()
            command.ExecuteNonQuery () |> ignore
            
            command.CommandText <- "CREATE DATABASE " + dbkey + " OWNER " + dbkey
            command.ExecuteNonQuery () |> ignore
        
            use specificConnection = AdminConnection.connectSpecific dbkey dbkey dbkey
            use specificCommand = connection.CreateCommand()

            specificCommand.CommandText <- "CREATE SCHEMA md"
            specificCommand.ExecuteNonQuery () |> ignore
        }
    
    let Storage = {
        register = registerUserWorker;
        listPending = listPendingUsersWorker;
        acceptRegistration = acceptRegistrationWorker;
        finishRegistration = finishRegistrationWorker;
        createDatabase = createUserDatabaseWorker;
    }