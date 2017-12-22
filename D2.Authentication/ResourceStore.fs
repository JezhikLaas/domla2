namespace D2.Authentication

open IdentityServer4.Stores

type ResourceStore (storage : ResourceStorage) =
    interface IResourceStore with

        member this.FindApiResourceAsync (name : string) =
            async {
                let! result = storage.findApiResource name
                match result with
                | Some r -> return r
                | None   -> return null
            }
            |> Async.StartAsTask
        
        member this.FindApiResourcesByScopeAsync (scopeNames : string seq) =
            storage.findApiResourcesByScope scopeNames |> Async.StartAsTask
        
        member this.FindIdentityResourcesByScopeAsync (scopeNames : string seq) =
            storage.findIdentityResourcesByScope scopeNames |> Async.StartAsTask
        
        member this.GetAllResourcesAsync () =
            storage.getAllResources () |> Async.StartAsTask