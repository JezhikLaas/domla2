namespace D2.UserManagement.Persistence

open D2.Common
open Mapper
open System
open System.Collections.Generic

module Users =
    
    let private isAccepted (user : UserRegistration) =
        async {
            use connection = Connection.client ()
            use command = connection.CreateCommand()
    
            command.CommandText <- """SELECT
                                          login, email
                                      FROM
                                          users
                                      WHERE
                                          email = :email
                                          OR
                                          login = :login"""
    
            command.Parameters.AddWithValue("email", user.EMail.ToLowerInvariant()) |> ignore
            command.Parameters.AddWithValue("login", user.Login.ToLowerInvariant()) |> ignore
    
            use! reader = command.ExecuteReaderAsync() |> Async.AwaitTask
    
            match reader.Read() with
            | false -> return RegistrationResult.Ok
            | true  -> if reader.GetString(1) = user.EMail.ToLower() then
                              return RegistrationResult.Known
                          else
                              return RegistrationResult.Conflict
        }
        
    let private registerUserWorker (user : UserRegistration) =
        let knownRegistration (client : Npgsql.NpgsqlConnection) =
            async {
                use command = client.CreateCommand()

                command.CommandText <- """SELECT
                                              login, email
                                          FROM
                                              registrations
                                          WHERE
                                              email = :email
                                              OR
                                              login = :login"""

                command.Parameters.AddWithValue("email", user.EMail.ToLowerInvariant()) |> ignore
                command.Parameters.AddWithValue("login", user.Login.ToLowerInvariant()) |> ignore
        
                use! reader = command.ExecuteReaderAsync() |> Async.AwaitTask

                match reader.Read() with
                | true  -> return Some (reader.GetString(0), reader.GetString(1))
                | false -> return None
            }
        async {
            use connection = Connection.client ()

            let! known = knownRegistration connection

            match known with
            | None   -> match isAccepted user |> Async.RunSynchronously with
                        | RegistrationResult.Ok -> use command = connection.CreateCommand()
                                                   command.CommandText <- """INSERT INTO
                                                                                 registrations(
                                                                                     id,
                                                                                     first_name,
                                                                                     last_name,
                                                                                     email,
                                                                                     title,
                                                                                     login,
                                                                                     salutation
                                                                                 )
                                                                             VALUES(
                                                                                 :id,
                                                                                 :first_name,
                                                                                 :last_name,
                                                                                 :email,
                                                                                 :title,
                                                                                 :login,
                                                                                 :salutation
                                                                             )"""
                                                   command.Parameters.AddWithValue("id", Guid.NewGuid()) |> ignore
                                                   command.Parameters.AddWithValue("first_name", user.FirstName) |> ignore
                                                   command.Parameters.AddWithValue("last_name", user.LastName) |> ignore
                                                   command.Parameters.AddWithValue("email", user.EMail.ToLowerInvariant()) |> ignore
                                                   command.Parameters.AddWithValue("title", user.Title) |> ignore
                                                   command.Parameters.AddWithValue("login", user.Login) |> ignore
                                                   command.Parameters.AddWithValue("salutation", user.Salutation) |> ignore
                                                   let! _ = command.ExecuteNonQueryAsync() |> Async.AwaitTask
                                                   return RegistrationResult.Ok
                        | result -> return result
            | Some u -> if snd u = user.EMail.ToLower() then
                            return RegistrationResult.Known
                        else
                            return RegistrationResult.Conflict
        }
    
    let private listPendingUsersWorker = 
        async {
            use connection = Connection.client ()
            use command = connection.CreateCommand()

            command.CommandText <- """SELECT
                                          id,
                                          first_name,
                                          last_name,
                                          email,
                                          title,
                                          login,
                                          salutation
                                      FROM
                                          registrations
                                      WHERE
                                          mail_sent IS NULL"""
        
            use! reader = command.ExecuteReaderAsync() |> Async.AwaitTask

            return seq {
            while reader.Read() do
                yield
                    new UserRegistrationI(
                        Id = reader.GetGuid(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        EMail = reader.GetString(3),
                        Title = reader.GetString(4),
                        Login = reader.GetString(5),
                        Salutation = reader.GetString(6)
                    )
                    :> UserRegistration
            }
            |> Seq.toList
        }
    
    let private markRegistrationsWorker (ids : Guid seq) =
        async {
            use connection = Connection.client ()
            use command = connection.CreateCommand()
            let idValues = ids |> Seq.toArray

            command.CommandText <- """UPDATE
                                          registrations
                                      SET
                                          mail_sent = :mail_sent
                                      WHERE
                                          id IN (:ids)"""
            command.Parameters.AddWithValue("mail_sent", NpgsqlTypes.NpgsqlDbType.Timestamp, DateTime.UtcNow) |> ignore
            command.Parameters.AddWithValue("ids", NpgsqlTypes.NpgsqlDbType.Array ||| NpgsqlTypes.NpgsqlDbType.Uuid, idValues) |> ignore
            let! _ = command.ExecuteNonQueryAsync() |> Async.AwaitTask
            ()
        }
    
    let Storage = {
        register = registerUserWorker;
        listPending = listPendingUsersWorker;
        markRegistrations = markRegistrationsWorker;
    }