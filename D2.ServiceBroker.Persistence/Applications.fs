namespace D2.ServiceBroker.Persistence

open Cast
open D2.Common
open Mapper
open Newtonsoft.Json
open Npgsql
open System
open System.Collections.Generic

module Applications =

    let private listApplicationsWorker =
        async {
            use connection = Connection.client ()
            use command = connection.CreateCommand()
            command.CommandText <- "SELECT name, version, patch FROM applications"
        
            use! configurations = command.ExecuteReaderAsync() |> Async.AwaitTask
        
            return seq {
                while configurations.Read() do
                    yield
                        new ApplicationI(
                            Id = Guid.Empty,
                            Name = configurations.GetString(0),
                            Version = configurations.GetInt32(1),
                            Patch = configurations.GetInt32(2),
                            Services = List.empty
                        )
                        :> Application
            }
            |> Seq.toList
        }

    let private listServicesWorker (name : string) (version : int) =
        async {
            use connection = Connection.client ()
            use command = connection.CreateCommand()
            command.CommandText <- """SELECT
                                          name, version, patch, route, tag
                                      FROM
                                          services
                                      WHERE application_name = :name AND application_version = :version"""
        
            command.Parameters.AddWithValue("name", name) |> ignore
            command.Parameters.AddWithValue("version", version) |> ignore

            use! services = command.ExecuteReaderAsync() |> Async.AwaitTask
        
            return seq {
                while services.Read() do
                    yield
                        new ServiceI(
                            Name = services.GetString(0),
                            BaseUrl = services.GetString(3),
                            Group = services.GetString(4),
                            Version = services.GetInt32(1),
                            Patch = services.GetInt32(2),
                            EndPoints = []
                        )
                        :> Service
            }
            |> Seq.toList
        }

    let monitor = new Object()
    
    let private ensureApplication (name : string) (version : int) =
        let applicationExists = 
            async {
                use connection = Connection.client ()
                use command = connection.CreateCommand()
                command.CommandText <- "SELECT 1 FROM applications WHERE name = :name AND version = :version"
                command.Parameters.AddWithValue("name", name) |> ignore
                command.Parameters.AddWithValue("version", version) |> ignore

                use! reader = command.ExecuteReaderAsync() |> Async.AwaitTask
                return reader.Read()
            }
        
        async {
            let! checkApplication = applicationExists
            match checkApplication with
            | true  -> ()
            | false -> use connection = Connection.client ()
                       use command = connection.CreateCommand()
                       command.CommandText <- "INSERT INTO applications(name, version, patch) VALUES (:name, :version, :patch)"
                       command.Parameters.AddWithValue("name", name) |> ignore
                       command.Parameters.AddWithValue("version", version) |> ignore
                       command.Parameters.AddWithValue("patch", 0) |> ignore
                       let! _ = command.ExecuteNonQueryAsync() |> Async.AwaitTask
                       ()
        }
    
    let private serviceInfoWorker (target : string * int) (service : string) =
        async {
            use connection = Connection.client ()
            use command = connection.CreateCommand()

            command.CommandText <- """SELECT
                                          version, patch, route, endpoints, tag
                                      FROM
                                          services
                                      WHERE
                                          application_name = :app_name
                                          AND
                                          application_version = :app_version
                                          AND
                                          name = :name"""
            
            command.Parameters.AddWithValue("app_name", fst target) |> ignore
            command.Parameters.AddWithValue("app_version", snd target) |> ignore
            command.Parameters.AddWithValue("name", service.ToLowerInvariant()) |> ignore

            use! reader = command.ExecuteReaderAsync() |> Async.AwaitTask

            match reader.Read() with
            | false -> return None
            | true  -> return Some(
                           new ServiceI(
                               Name = service,
                               BaseUrl = reader.GetString(2),
                               Group = reader.GetString(4),
                               Version = reader.GetInt32(0),
                               Patch = reader.GetInt32(1),
                               EndPoints = JsonConvert.DeserializeObject<EndPointI list>(reader.GetString(3))
                           )
                           :> Service
                       )
        }

    let fetchPatchLevel (target : string * int) (service : Service) (connection : NpgsqlConnection) =
        async {
            use command = connection.CreateCommand()
            command.CommandText <- """SELECT
                                          patch
                                      FROM
                                          services
                                      WHERE
                                          application_name = :app_name
                                          AND
                                          application_version = :app_version
                                          AND
                                          name = :name
                                          AND
                                          version = :version"""
        
            command.Parameters.AddWithValue("app_name", fst target) |> ignore
            command.Parameters.AddWithValue("app_version", snd target) |> ignore
            command.Parameters.AddWithValue("name", service.Name.ToLowerInvariant()) |> ignore
            command.Parameters.AddWithValue("version", service.Version) |> ignore
        
            use! patch = command.ExecuteReaderAsync() |> Async.AwaitTask
            match patch.Read() with
            | false -> return -1
            | true  -> return patch.GetInt32(0)
        }
    
    let insertOrReplaceService (target : string * int) (service : Service) (connection : NpgsqlConnection) =
        async {
            use command = connection.CreateCommand()
            command.CommandText <- """INSERT INTO
                                          services(
                                              application_name,
                                              application_version,
                                              name,
                                              tag,
                                              version,
                                              patch,
                                              route,
                                              endpoints
                                          )
                                      VALUES(
                                          :application_name,
                                          :application_version,
                                          :name,
                                          :tag,
                                          :version,
                                          :patch,
                                          :route,
                                          :endpoints
                                      )
                                      ON CONFLICT
                                          (application_name, application_version, tag, name, version)
                                      DO UPDATE SET
                                          patch = EXCLUDED.patch, route = EXCLUDED.route, endpoints = EXCLUDED.endpoints"""
               
            let endpoints = service.EndPoints
                            |> List.map(fun e -> EndPointI.fromEndpoint(e))
                            |> List.toArray
        
            let endpointString = JsonConvert.SerializeObject(endpoints)
        
            command.Parameters.AddWithValue("application_name", fst target) |> ignore
            command.Parameters.AddWithValue("application_version", snd target) |> ignore
            command.Parameters.AddWithValue("name", service.Name.ToLowerInvariant()) |> ignore
            command.Parameters.AddWithValue("tag", service.Group.ToLowerInvariant()) |> ignore
            command.Parameters.AddWithValue("version", service.Version) |> ignore
            command.Parameters.AddWithValue("patch", service.Patch) |> ignore
            command.Parameters.AddWithValue("route", service.BaseUrl) |> ignore
            command.Parameters.AddWithValue("endpoints", NpgsqlTypes.NpgsqlDbType.Jsonb, endpointString) |> ignore

            let! _ = command.ExecuteNonQueryAsync() |> Async.AwaitTask
            ()
        }
    
    let private registerServiceWorker (target : string * int) (service : Service) =
        async {
            do! ensureApplication (fst target) (snd target)

            use connection = Connection.client ()
            use transaction = connection.BeginTransaction()

            let! patchLevel = fetchPatchLevel target service connection
            match patchLevel with
            | -1 -> do! insertOrReplaceService target service connection
            
            | pt when pt < service.Patch
                    ->
                    do! insertOrReplaceService target service connection
                    use update = connection.CreateCommand()
                    update.CommandText <- "UPDATE applications SET patch = patch + 1"
                    let! _ = update.ExecuteNonQueryAsync() |> Async.AwaitTask
                    ()
            
            | _  -> ()
            
            transaction.Commit()
        }

    let Storage = {
        services = listServicesWorker
        applications = listApplicationsWorker
        register = registerServiceWorker
        endpoints = serviceInfoWorker
    }