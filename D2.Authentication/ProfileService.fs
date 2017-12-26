namespace D2.Authentication

open IdentityServer4.Models
open IdentityServer4.Services
open System.Threading.Tasks

type ProfileService (storage : PersistedGrantStorage) =
    interface IProfileService with

        member this.GetProfileDataAsync (context : ProfileDataRequestContext) = 
            Task.CompletedTask
        
        member this.IsActiveAsync(context : IsActiveContext) =
            Task.CompletedTask
