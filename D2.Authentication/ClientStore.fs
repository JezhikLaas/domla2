namespace D2.Authentication

open IdentityServer4.Models
open IdentityServer4.Stores
open System.Threading.Tasks

type ClientStore (storage : ClientStorage) =
    interface IClientStore with
        
        member this.FindClientByIdAsync (clientId : string) =
            async {
                let! result = storage.findClientById clientId
                match result with
                | Some r -> return r
                | None   -> return null
            }
            |> Async.StartAsTask

