namespace D2.Authentication

open IdentityServer4.Extensions
open IdentityServer4.Models
open IdentityServer4.Services
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open System.Security.Claims

type ProfileService (storage : UserStorage) =
    interface IProfileService with

        member this.GetProfileDataAsync (context : ProfileDataRequestContext) = 
            async {
                let! result = storage.fetchUser (context.Subject.GetSubjectId())
                match result with
                | Some user -> context.IssuedClaims <- List<Claim> (
                                                           user.Claims
                                                           |>
                                                           List.filter(
                                                               fun claim
                                                                   ->
                                                                   context.RequestedClaimTypes = null
                                                                   ||
                                                                   context
                                                                       .RequestedClaimTypes
                                                                       .Contains(claim.Type)
                                                           )
                                                       )
                | None -> ()
            } |> Async.StartAsTask :> Task
        
        member this.IsActiveAsync(context : IsActiveContext) =
            async {
                let! result = storage.fetchUser (context.Subject.GetSubjectId())
                match result with
                | Some user -> context.IsActive <- user.LoggedIn
                | None      -> context.IsActive <- false
            } |> Async.StartAsTask :> Task
