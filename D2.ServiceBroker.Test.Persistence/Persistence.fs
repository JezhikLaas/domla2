namespace D2ServiceBrokerPersistenceTest

open D2.Common
open D2.ServiceBroker.Persistence
open FsUnit
open Npgsql
open NUnit.Framework

[<TestFixture>]
[<Category("Persistence")>]
module StorageTest = 

    [<SetUp>]
    let ``For each test setup`` () =
        try
            let builder = new NpgsqlConnectionStringBuilder()
            builder.Database <- "D2.ServiceBroker"
            builder.Host <- "localhost"
            builder.Password <- "d2broker"
            builder.Username <- "d2broker"
            builder.Port <- 5432
            
            use connection = new NpgsqlConnection(builder.ConnectionString)
            connection.Open()
            
            use command = connection.CreateCommand()
            command.CommandText <- """CREATE SCHEMA test_sb AUTHORIZATION d2broker
                                          CREATE TABLE applications (
                                              name character varying(255) NOT NULL,
                                              version integer NOT NULL,
                                              patch integer NOT NULL,
                                              PRIMARY KEY (name, version)
                                          )
                                          CREATE TABLE services (
                                              application_name character varying(255) NOT NULL,
                                              application_version integer NOT NULL,
                                              name character varying(255) NOT NULL,
                                              version integer NOT NULL,
                                              patch integer NOT NULL,
                                              route character varying(512) NOT NULL,
                                              endpoints jsonb NOT NULL,
                                              PRIMARY KEY (application_name, application_version, name, version),
                                              FOREIGN KEY (application_name, application_version) REFERENCES applications(name, version) DEFERRABLE INITIALLY DEFERRED
                                          )
                                          """
            command.ExecuteNonQuery() |> ignore
            
            Connection.connectionProvider <-
                fun () -> 
                    let builder = new NpgsqlConnectionStringBuilder()
                    builder.Database <- "D2.ServiceBroker"
                    builder.Host <- "localhost"
                    builder.Password <- "d2broker"
                    builder.Username <- "d2broker"
                    builder.Port <- 5432
                    builder.SearchPath <- "test_sb"
                    
                    new NpgsqlConnection(builder.ConnectionString)
        with
        | _ -> ()
        ()
    
    [<TearDown>]
    let ``After each test tear down`` () =
        try
            let builder = new NpgsqlConnectionStringBuilder()
            builder.Database <- "D2.ServiceBroker"
            builder.Host <- "localhost"
            builder.Password <- "d2broker"
            builder.Username <- "d2broker"
            builder.Port <- 5432
            
            use connection = new NpgsqlConnection(builder.ConnectionString)
            connection.Open()
            
            use command = connection.CreateCommand()
            command.CommandText <- "DROP SCHEMA IF EXISTS test_sb CASCADE"
            
            command.ExecuteNonQuery() |> ignore
        with
        | _ -> ()
        ()

    [<Test>]
    let ``Register microservice`` () =
        let service = {
            new Service with
            member this.Name = "Test"
            member this.BaseUrl = "http://localhost:2300"
            member this.Version = 1
            member this.Patch = 0
            member this.EndPoints = []
        }
        
        Applications.Storage.register ("Domla2", 1) service |> Async.RunSynchronously

        match Applications.Storage.applications |> Async.RunSynchronously with
        | application -> application.Length |> should equal 1
    
    [<Test>]
    let ``List microservices`` () =
        let service = {
            new Service with
            member this.Name = "Test"
            member this.BaseUrl = "http://localhost:2300"
            member this.Version = 1
            member this.Patch = 0
            member this.EndPoints = []
        }
        
        Applications.Storage.register ("Domla2", 1) service |> Async.RunSynchronously
        
        let services = Applications.Storage.services "Domla2" 1 |> Async.RunSynchronously
        match services with
        | s -> s.Length |> should equal 1
    
    [<Test>]
    [<Category("WI 58")>]
    let ``Adding the same service / version / patch twice is ignored`` () =
        let service = {
            new Service with
            member this.Name = "Test"
            member this.BaseUrl = "http://localhost:2300"
            member this.Version = 1
            member this.Patch = 0
            member this.EndPoints = []
        }
        
        Applications.Storage.register ("Domla2", 1) service |> Async.RunSynchronously
        Applications.Storage.register ("Domla2", 1) service |> Async.RunSynchronously
        
        let services = Applications.Storage.services "Domla2" 1  |> Async.RunSynchronously
        match services with
        | s -> s.Length |> should equal 1
    
    [<Test>]
    [<Category("WI 58")>]
    let ``Adding the same service / version with higher patch replaces the previous entry`` () =
        let serviceOne = {
            new Service with
            member this.Name = "Test"
            member this.BaseUrl = "http://localhost:2300"
            member this.Version = 1
            member this.Patch = 0
            member this.EndPoints = []
        }
        
        let serviceTwo = {
            new Service with
            member this.Name = "Test"
            member this.BaseUrl = "http://localhost:2301"
            member this.Version = 1
            member this.Patch = 1
            member this.EndPoints = []
        }
        
        Applications.Storage.register ("Domla2", 1) serviceOne |> Async.RunSynchronously
        Applications.Storage.register ("Domla2", 1) serviceTwo |> Async.RunSynchronously
        
        let services = Applications.Storage.services "Domla2" 1 |> Async.RunSynchronously
        match services with
        | s -> s.Length |> should equal 1
               s.[0].Patch |> should equal 1
    
    [<Test>]
    [<Category("WI 58")>]
    let ``Adding the same service / version with lower patch is ignored`` () =
        let serviceOne = {
            new Service with
            member this.Name = "Test"
            member this.BaseUrl = "http://localhost:2300"
            member this.Version = 1
            member this.Patch = 1
            member this.EndPoints = []
        }
        
        let serviceTwo = {
            new Service with
            member this.Name = "Test"
            member this.BaseUrl = "http://localhost:2301"
            member this.Version = 1
            member this.Patch = 0
            member this.EndPoints = []
        }
        
        Applications.Storage.register ("Domla2", 1) serviceOne |> Async.RunSynchronously
        Applications.Storage.register ("Domla2", 1) serviceTwo |> Async.RunSynchronously
        
        let services = Applications.Storage.services "Domla2" 1 |> Async.RunSynchronously
        match services with
        | s -> s.Length |> should equal 1
               s.[0].Patch |> should equal 1
    
    [<Test>]
    [<Category("WI 58")>]
    let ``Adding the same service / version with higher patch increases application patch level`` () =
        let serviceOne = {
            new Service with
            member this.Name = "Test"
            member this.BaseUrl = "http://localhost:2300"
            member this.Version = 1
            member this.Patch = 0
            member this.EndPoints = []
        }
        
        let serviceTwo = {
            new Service with
            member this.Name = "Test"
            member this.BaseUrl = "http://localhost:2301"
            member this.Version = 1
            member this.Patch = 1
            member this.EndPoints = []
        }
        
        Applications.Storage.register ("Domla2", 1) serviceOne |> Async.RunSynchronously
        
        match Applications.Storage.applications |> Async.RunSynchronously with
        | a -> a.Length |> should equal 1
               a.[0].Patch |> should equal 0
        
        Applications.Storage.register ("Domla2", 1) serviceTwo |> Async.RunSynchronously
        
        match Applications.Storage.applications |> Async.RunSynchronously with
        | a -> a.Length |> should equal 1
               a.[0].Patch |> should equal 1

    [<Test>]
    let ``Refetch endpoint`` () =
        let endpoint = {
            new EndPoint with 
            member this.Name = "123"
            member this.Uri = "/"
        }
        let service = {
            new Service with
            member this.Name = "Test"
            member this.BaseUrl = "http://localhost:2300"
            member this.Version = 1
            member this.Patch = 0
            member this.EndPoints = [endpoint]
        }
        
        Applications.Storage.register ("Domla2", 1) service |> Async.RunSynchronously

        match Applications.Storage.endpoints ("Domla2", 1) "test" |> Async.RunSynchronously with
        | Some service -> service.BaseUrl |> should equal "http://localhost:2300"
                          service.EndPoints.Length |> should equal 1
                          service.EndPoints.[0].Name |> should equal "123"
                          service.EndPoints.[0].Uri |> should equal "/"
        | _ -> failwith "unknown error"
