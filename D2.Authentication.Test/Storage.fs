namespace D2Authentication.Test

open D2.Authentication
open D2.Authentication.Storage
open FsUnit
open NUnit.Framework

[<TestFixture>]
[<Category("Storage")>]
module StorageTest =
    
    let fetchStorage () =
        let connectionOptions = {
            Database = "D2.Authentication";
            Host = "janeway";
            User = "d2admin";
            Password = "d2admin";
            Port = 5433;
        }
        
        let setupStorage = Storage.storages.setupStorage connectionOptions;
        setupStorage.initialize () |> Async.RunSynchronously
        
        Storage.storages.userStorage connectionOptions

    [<Test>]
    let ``Admin can login with well known password`` () =
        let storage = fetchStorage ()
        let result = storage.findUser "admin" "secret" |> Async.RunSynchronously 
        match result with
        | Some user -> user.Login |> should equal "admin"
        | None      -> failwith "unable to find admin"
        ()

    [<Test>]
    let ``Admin with invalid password is rejected`` () =
        let storage = fetchStorage ()
        match storage.findUser "admin" "whatever" |> Async.RunSynchronously with
        | Some _ -> failwith "login succeeded with invalid pwd"
        | None   -> ()
        ()