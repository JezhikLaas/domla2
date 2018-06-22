namespace D2.UserManagement.Persistence.Test

open Beginor.NHibernate.NpgSql
open D2.Common
open D2.UserManagement.Persistence
open FsUnit
open FluentNHibernate.Cfg
open NHibernate.Mapping
open NHibernate.Tool.hbm2ddl
open Npgsql
open NUnit.Framework
open System
open System.IO

[<TestFixture>]
[<Category("Persistence")>]
module StorageTest =

    let mutable testFile = String.Empty 
    
    let buildSchema (config : NHibernate.Cfg.Configuration) =
        let versioned = config.ClassMappings
                        |> Seq.filter (fun cm -> cm.Version <> null)
                        |> Seq.toList

        for mapping in versioned do
            mapping.Version.Generation <- PropertyGeneration.Never
            mapping.Version.IsInsertable <- true

        (SchemaExport(config)).Create(false, true) |> ignore

    [<SetUp>]
    let ``For each test setup`` () =
        testFile <- Path.GetTempFileName()
        let configuration = Fluently.Configure()
                                    .Database(SqliteConfiguration.Standard.UsingFile(testFile))
                                    .ExposeConfiguration(fun config -> buildSchema config)
        Connection.initialize configuration
    
    [<TearDown>]
    let ``After each test tear down`` () =
        Connection.shutdown ()
        
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
            member this.MailSent = None
            member this.Version = 0
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
            member this.MailSent = None
            member this.Version = 0
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
            member this.MailSent = None
            member this.Version = 0
        }
        
        Users.Storage.register user  |> Async.RunSynchronously |> ignore

        match Users.Storage.listPending |> Async.RunSynchronously  with
        | pending -> pending.Head.EMail |> should equal user.EMail
                     pending.Head.FirstName |> should equal user.FirstName
                     pending.Head.LastName |> should equal user.LastName
                     pending.Head.Salutation |> should equal user.Salutation
                     pending.Head.Title |> should equal user.Title
                     pending.Head.Login |> should equal user.Login
