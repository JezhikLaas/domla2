namespace D2UserManagementTest

open NUnit.Framework

[<TestFixture>]
[<Category("Validate registration")>]
module ValidationTest =
    open D2.UserManagement
    open FsUnit
    
    [<Test>]
    let ``Accept simple email as valid``() =
        Registration.isValidEmailAddress "user@example.com" |> should equal (true)
    
    [<Test>]
    let ``Accept long TLD as valid``() =
        Registration.isValidEmailAddress "user@example.museum" |> should equal (true)
    
    [<Test>]
    let ``Email with missing TLD is invalid``() =
        Registration.isValidEmailAddress "user@example" |> should equal (false)
    
    [<Test>]
    let ``Email without user part is invalid``() =
        Registration.isValidEmailAddress "@example.museum" |> should equal (false)
    
    [<Test>]
    let ``Login consisting of accepted chars is valid``() =
        Registration.isValidLogin "a123-5" |> should equal (true)
    
    [<Test>]
    let ``Login with space is invalid``() =
        Registration.isValidLogin "a123 5" |> should equal (false)
    
    [<Test>]
    let ``Login with underscore is valid``() =
        Registration.isValidLogin "a123_5" |> should equal (true)
    
    [<Test>]
    let ``Login with leading number is invalid``() =
        Registration.isValidLogin "3a123_5" |> should equal (false)
    
    [<Test>]
    let ``Login with upper case chars is invalid``() =
        Registration.isValidLogin "Abc" |> should equal (false)
