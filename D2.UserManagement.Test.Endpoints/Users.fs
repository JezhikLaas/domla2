namespace D2.UserManagement.Test.Endpoints

open D2.Common
open D2.UserManagement
open D2.UserManagement.Persistence
open FsUnit
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.TestHost
open Microsoft.Extensions.Logging
open NUnit.Framework
open System
open System.Collections.Generic
open System.Net.Http

[<TestFixture>]
[<Category("Endpoints")>]
module RegistrationTest = 

    let private dummyData = new List<UserRegistration>()

    let private registerUser (target : UserRegistration) =
        async {
            return match dummyData.FindIndex(fun u -> u.EMail = target.EMail) with 
                   | -1 -> match dummyData.FindIndex(fun u -> u.Login = target.Login) with
                           | -1 -> dummyData.Add(target)
                                   Ok
                           | _  -> Conflict
                   | _  -> Known
        }
    
    let private listPendingUsers =
        async {
            return List.empty
        }
    
    let private acceptRegistrationTest (item : Guid) (logger : ILogger) (prerequisite : (UserRegistration -> Async<bool>)) =
        async {
            return true
        }
    
    let private finishRegistrationTest (item : Guid) (password : string) (logger : ILogger) =
        async {
            return true
        }
    
    type UserRegistrationI() =
        member val Id = Guid.Empty with get, set
        member val FirstName = String.Empty with get, set
        member val LastName = String.Empty with get, set
        member val Salutation = String.Empty with get, set
        member val Title = String.Empty with get, set
        member val EMail = String.Empty with get, set
        member val Login = String.Empty with get, set
        member val MailSent = None with get, set
        member val Version = 0 with get, set
        
        interface UserRegistration with
            member this.Id with get() = this.Id
            member this.FirstName with get() = this.FirstName
            member this.LastName with get() = this.LastName
            member this.Salutation with get() = this.Salutation
            member this.Title with get() = this.Title
            member this.EMail with get() = this.EMail
            member this.Login with get() = this.Login
            member this.MailSent with get() = this.MailSent
            member this.Version with get() = this.Version

    let testStorage = {
        register = registerUser
        listPending = listPendingUsers
        acceptRegistration = acceptRegistrationTest
        finishRegistration = finishRegistrationTest
    }
    
    let mutable browser = null

    [<OneTimeSetUp>]
    let setupOnce() =
        CompositionRoot.setStorage testStorage
        let server = new TestServer((WebHostBuilder()).UseStartup<StartupTest>())
        browser <- server.CreateClient()

    [<SetUp>]
    let ``For each test setup`` () =
        dummyData.Clear()
    
    [<Test>]
    let ``User Management answers Ok when user can be inserted``() =
        let user = UserRegistrationI(
                       FirstName = "foo",
                       LastName = "bar",
                       EMail = "foo@example.com",
                       Salutation = "Mrs",
                       Login = "foobar"
                   )
        let body = Newtonsoft.Json.JsonConvert.SerializeObject(user)
        let response = browser.PutAsync("/users/register", new StringContent(body, Text.Encoding.UTF8, "application/json"))
                       |> Async.AwaitTask
                       |> Async.RunSynchronously
        let result = response.StatusCode

        int result |> should equal StatusCodes.Status200OK
    
    [<Test>]
    let ``User Management answers MovedPermanently when email is in use``() =
        let user = UserRegistrationI(
                       FirstName = "foo",
                       LastName = "bar",
                       EMail = "foo@example.com",
                       Salutation = "Mrs",
                       Login = "foobar"
                   )
        let body = Newtonsoft.Json.JsonConvert.SerializeObject(user)
        browser.PutAsync("/users/register", new StringContent(body, Text.Encoding.UTF8, "application/json"))
        |> Async.AwaitTask
        |> Async.RunSynchronously
        |> ignore

        user.Login <- "barfoo"
        
        let body = Newtonsoft.Json.JsonConvert.SerializeObject(user)
        let response = browser.PutAsync("/users/register", new StringContent(body, Text.Encoding.UTF8, "application/json"))
                       |> Async.AwaitTask
                       |> Async.RunSynchronously
        let result = response.StatusCode

        int result |> should equal StatusCodes.Status301MovedPermanently
    
    [<Test>]
    let ``User Management answers Conflict when login is in use``() =
        let user = UserRegistrationI(
                       FirstName = "foo",
                       LastName = "bar",
                       EMail = "foo@example.com",
                       Salutation = "Mrs",
                       Login = "foobar"
                   )
        let body = Newtonsoft.Json.JsonConvert.SerializeObject(user)
        browser.PutAsync("/users/register", new StringContent(body, Text.Encoding.UTF8, "application/json"))
        |> Async.AwaitTask
        |> Async.RunSynchronously
        |> ignore

        user.EMail <- "bar@example.com"
        
        let body = Newtonsoft.Json.JsonConvert.SerializeObject(user)
        let response = browser.PutAsync("/users/register", new StringContent(body, Text.Encoding.UTF8, "application/json"))
                       |> Async.AwaitTask
                       |> Async.RunSynchronously
        let result = response.StatusCode

        int result |> should equal StatusCodes.Status409Conflict
