namespace D2.Authentication

module UserData = 
    
    open BCrypt.Net
    open D2.Common
    open Json
    open Npgsql
    open System
    open System.Data.Common

    let private findUser (options : ConnectionOptions) (name : string) (password : string) =
        async {
            use connection = authentication options
            use command = connection.CreateCommand()
    
            command.CommandText <- """SELECT
                                          id,
                                          login,
                                          first_name,
                                          last_name,
                                          email,
                                          password,
                                          title,
                                          salutation,
                                          claims,
                                          logged_in
                                      FROM
                                          users
                                      WHERE
                                          login = :login"""
            command.Parameters << ("login", StringField name) |> ignore
    
            use! reader = command.ExecuteReaderAsync() |> Async.AwaitTask
            match reader.Read() with
            | true  -> let storedPassword = reader.GetString(5)
                       match BCrypt.Verify(password, storedPassword) with
                       | true  -> return Some (UserI.fromReader reader)
                       | false -> return None
            | false -> return None
        }
    
    let private fetchUser (options : ConnectionOptions) (id : string) =
        async {
            use connection = authentication options
            use command = connection.CreateCommand()
    
            command.CommandText <- """SELECT
                                          id,
                                          login,
                                          first_name,
                                          last_name,
                                          email,
                                          title,
                                          salutation,
                                          claims,
                                          logged_in
                                      FROM
                                          users
                                      WHERE
                                          login = :login"""
            command.Parameters << ("login", StringField id) |> ignore
    
            use! reader = command.ExecuteReaderAsync() |> Async.AwaitTask
            match reader.Read() with
            | true  -> return Some (UserI.fromReader reader)
            | false -> return None
        }
        
    let private updateActive (options : ConnectionOptions) (id : string) (state : bool) =
        async {
            use connection = authentication options
            use command = connection.CreateCommand()
    
            command.CommandText <- sprintf """UPDATE
                                                  users
                                              SET
                                                  logged_in = %s
                                              WHERE
                                                  id = :id""" (if state then "LOCALTIMESTAMP" else "NULL")
            command.Parameters << ("id", GuidField (new Guid(id))) |> ignore
    
            let! result = command.ExecuteNonQueryAsync() |> Async.AwaitTask
            match result with
            | 1 -> return ()
            | _ -> failwith "failed to update login state"
        }

    let access (options : ConnectionOptions) =
        {
            findUser = findUser options;
            fetchUser = fetchUser options;
            updateActive = updateActive options;
        }
        

