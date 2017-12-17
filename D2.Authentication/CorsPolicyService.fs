namespace D2.Authentication

open IdentityServer4.Services
open System.Threading.Tasks

type CorsPolicyService (storage : AuthorizationStorage) =
    interface ICorsPolicyService with
        
        member this.IsOriginAllowedAsync (origin : string) =
            Task.FromResult true