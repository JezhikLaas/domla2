namespace D2.Authentication

open IdentityServer4.Models
open IdentityServer4.Stores
open System.Threading.Tasks

type AuthorizationCodeStore(storage : AuthorizationStorage) =
    interface IAuthorizationCodeStore with
        member this.StoreAuthorizationCodeAsync(value : AuthorizationCode) =
            async {
                return! storage.storeAuthorizationCode value
            }
            |> Async.StartAsTask
        
        member this.GetAuthorizationCodeAsync(key : string) =
            async {
                let! result = storage.getAuthorizationCode key
                match result with
                | Some c -> return c
                | None   -> return null
            }
            |> Async.StartAsTask

        member this.RemoveAuthorizationCodeAsync(key : string) =
            async {
                do! storage.deleteAuthorizationCode key
            }
            |> Async.StartAsTask :> Task
