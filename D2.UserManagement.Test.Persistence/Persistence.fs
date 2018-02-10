namespace D2.UserManagement.Persistence.Test

open D2.Common
open D2.UserManagement.Persistence
open FsUnit
open Npgsql
open NUnit.Framework
open System

[<TestFixture>]
[<Category("Persistence")>]
module StorageTest = 

    [<SetUp>]
    let ``For each test setup`` () =
        try
            let builder = new NpgsqlConnectionStringBuilder()
            builder.Database <- "D2.Authentication";
            builder.Port <- 5433
            builder.Host <- "janeway"
            builder.Username <- "d2admin"
            builder.Password <- "d2admin" 
            
            use connection = new NpgsqlConnection(builder.ConnectionString)
            connection.Open()
            
            use command = connection.CreateCommand()
            command.CommandText <- """CREATE SCHEMA test_um AUTHORIZATION d2admin
                                          CREATE TABLE users
                                          (
                                              id uuid NOT NULL,
                                              login character varying(255) COLLATE pg_catalog."default" NOT NULL,
                                              first_name character varying(255) COLLATE pg_catalog."default",
                                              last_name character varying(255) COLLATE pg_catalog."default" NOT NULL,
                                              title character varying COLLATE pg_catalog."default",
                                              salutation character varying COLLATE pg_catalog."default",
                                              email character varying(255) COLLATE pg_catalog."default" NOT NULL,
                                              logged_in time without time zone,
                                              claims jsonb,
                                              password character varying(255) COLLATE pg_catalog."default" NOT NULL,
                                              CONSTRAINT users_pkey PRIMARY KEY (id)
                                          )
                                          CREATE TABLE registrations
                                          (
                                              login character varying(255) COLLATE pg_catalog."default" NOT NULL,
                                              first_name character varying(255) COLLATE pg_catalog."default",
                                              last_name character varying(255) COLLATE pg_catalog."default" NOT NULL,
                                              email character varying(255) COLLATE pg_catalog."default" NOT NULL,
                                              salutation character varying(50) COLLATE pg_catalog."default",
                                              title character varying(50) COLLATE pg_catalog."default",
                                              id uuid NOT NULL,
                                              CONSTRAINT registrations_pkey PRIMARY KEY (login)
                                          )
                                          """
            command.ExecuteNonQuery() |> ignore
            
            Connection.connectionProvider <-
                fun () -> 
                    let builder = new NpgsqlConnectionStringBuilder()
                    builder.Database <- "D2.Authentication";
                    builder.Port <- 5433
                    builder.Host <- "janeway"
                    builder.Username <- "d2admin"
                    builder.Password <- "d2admin" 
                    builder.SearchPath <- "test_um"
                    
                    new NpgsqlConnection(builder.ConnectionString)
        with
        | _ -> ()
        ()
    
    [<TearDown>]
    let ``After each test tear down`` () =
        try
            let builder = new NpgsqlConnectionStringBuilder()
            builder.Database <- "D2.Authentication";
            builder.Port <- 5433
            builder.Host <- "janeway"
            builder.Username <- "d2admin"
            builder.Password <- "d2admin" 
            
            use connection = new NpgsqlConnection(builder.ConnectionString)
            connection.Open()
            
            use command = connection.CreateCommand()
            command.CommandText <- "DROP SCHEMA IF EXISTS test_um CASCADE"
            
            command.ExecuteNonQuery() |> ignore
        with
        | _ -> ()
        ()

    [<Test>]
    let ``Registering unknown user succeeds`` () =
        let user = {
            new UserRegistration with
            member this.Id = Guid.Empty
            member this.FirstName = "foo"
            member this.LastName = "bar"
            member this.Salutation = "Herr"
            member this.Title = String.Empty
            member this.EMail = "foo@example.com"
            member this.Login = "foo"
        }
        
        let target = Users.Storage.register user |> Async.RunSynchronously
        target |> should equal RegistrationResult.Ok

    [<Test>]
    let ``After registration user is listed`` () =
        let user = {
            new UserRegistration with
            member this.Id = Guid.Empty
            member this.FirstName = "foo"
            member this.LastName = "bar"
            member this.Salutation = "Herr"
            member this.Title = String.Empty
            member this.EMail = "foo@example.com"
            member this.Login = "foo"
        }
        
        Users.Storage.register user |> Async.RunSynchronously |>ignore

        match Users.Storage.listPending |> Async.RunSynchronously with
        | pending -> pending |> should haveLength 1

    [<Test>]
    let ``After registration listed user is identical to inserted`` () =
        let user = {
            new UserRegistration with
            member this.Id = Guid.Empty
            member this.FirstName = "foo"
            member this.LastName = "bar"
            member this.Salutation = "Herr"
            member this.Title = String.Empty
            member this.EMail = "foo@example.com"
            member this.Login = "foo"
        }
        
        Users.Storage.register user  |> Async.RunSynchronously |> ignore

        match Users.Storage.listPending |> Async.RunSynchronously  with
        | pending -> pending.Head.EMail |> should equal user.EMail
                     pending.Head.FirstName |> should equal user.FirstName
                     pending.Head.LastName |> should equal user.LastName
                     pending.Head.Salutation |> should equal user.Salutation
                     pending.Head.Title |> should equal user.Title
                     pending.Head.Login |> should equal user.Login
