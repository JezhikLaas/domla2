namespace D2.UserManagement

open D2.Common
open Newtonsoft.Json
open NLog
open System

module ServiceRegistration =
    let private logger = LogManager.GetLogger "D2.UserManagement.ServiceRegistration"

    type EndPoint() =
        member val Name = String.Empty with get, set
        member val Uri = String.Empty with get, set

    type Service() =
        member val Name = "UserManagement" with get
        member val BaseUrl = (
                              ServiceConfiguration.configuration.Hosting
                              |> Seq.head
                             ).FullAddress
                             
        member val Version = ServiceConfiguration.versionInfo.Version with get
        member val Patch = ServiceConfiguration.versionInfo.Patch with get
        member val EndPoints = [|
                                   EndPoint(Name = "Frontend", Uri = "/");
                                   EndPoint(Name = "Register", Uri = "/users/register");
                               |] with get

    let registerSelf () =
        let textData = JsonConvert.SerializeObject(Service ())

        ServiceRegistrator.registerSelf null textData
