namespace D2.Authentication

open IdentityServer4.Models
open IdentityServer4.Stores
open System.Threading.Tasks

type ClientStore (storage : AuthorizationStorage) =
    interface IClientStore with
        
        member this.FindClientByIdAsync (clientId : string) =
            Task.FromResult (new Client ())

