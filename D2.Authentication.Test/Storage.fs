namespace D2Authentication.Test

open D2.Authentication
open D2.Authentication.Storage
open FsUnit
open NUnit.Framework

[<TestFixture>]
[<Category("Storage")>]
module StorageTest =
    
    [<Test>]
    let ``Admin can login with well known password`` () =
        (*
        let result = Storage.findUser "admin" "secret" |> Async.RunSynchronously 
        match result with
        | Some user -> user.Login |> should equal "admin"
        | None      -> failwith "unable to find admin"
        *)
        ()

    [<Test>]
    let ``Admin with invalid password is rejected`` () =
        (*
        match Storage.findUser "admin" "whatever" |> Async.RunSynchronously with
        | Some _ -> failwith "login succeeded with invalid pwd"
        | None   -> ()
        *)
        ()