namespace D2.Authentication

open IdentityServer4.Models
open IdentityServer4.Stores
open System.Threading.Tasks

type PersistedGrantStore (storage : AuthorizationStorage) =
    interface IPersistedGrantStore with
        
        member this.GetAllAsync (subjectId : string) =
            Task.FromResult (Seq.empty<PersistedGrant>)
        
        member this.GetAsync (key : string) =
            Task.FromResult (new PersistedGrant())
        
        member this.RemoveAllAsync (subjectId : string, clientId : string) =
            Task.CompletedTask
        
        member this.RemoveAllAsync (subjectId : string, clientId : string, grantType : string) =
            Task.CompletedTask
        
        member this.RemoveAsync (key : string) =
            Task.CompletedTask
        
        member this.StoreAsync (grant : PersistedGrant) =
            Task.CompletedTask