namespace D2.Authentication

open IdentityServer4.Models
open IdentityServer4.Stores
open System.Threading.Tasks

type PersistedGrantStore (storage : PersistedGrantStorage) =
    interface IPersistedGrantStore with
        
        member this.GetAllAsync (subjectId : string) =
            storage.getAll subjectId
            |> Async.StartAsTask
        
        member this.GetAsync (key : string) =
            async {
                let! item = storage.get key
                match item with
                | Some p -> return p
                | None   -> return null
            }
            |> Async.StartAsTask
        
        member this.RemoveAllAsync (subjectId : string, clientId : string) =
            storage.removeAll subjectId clientId
            |> Async.StartAsTask
            :> Task
        
        member this.RemoveAllAsync (subjectId : string, clientId : string, grantType : string) =
            storage.removeAllType subjectId clientId grantType
            |> Async.StartAsTask
            :> Task
        
        member this.RemoveAsync (key : string) =
            storage.remove key
            |> Async.StartAsTask
            :> Task
        
        member this.StoreAsync (grant : PersistedGrant) =
            storage.store grant
            |> Async.StartAsTask
            :> Task
