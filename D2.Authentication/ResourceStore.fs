namespace D2.Authentication

open IdentityServer4.Models
open IdentityServer4.Stores
open System.Threading.Tasks

type ResourceStore (storage : AuthorizationStorage) =
    interface IResourceStore with

        member this.FindApiResourceAsync (name : string) =
            Task.FromResult (new ApiResource())
        
        member this.FindApiResourcesByScopeAsync (scopeNames : string seq) =
            Task.FromResult (Seq.empty<ApiResource>)
        
        member this.FindIdentityResourcesByScopeAsync (scopeNames : string seq) =
            Task.FromResult (Seq.empty<IdentityResource>)
        
        member this.GetAllResourcesAsync () =
            Task.FromResult (new Resources())