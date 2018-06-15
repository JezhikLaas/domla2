namespace D2.Welcome

open D2.UserManagement.Persistence

module CompositionRoot =

    let mutable private finishFunc =
        Users.Storage.finishRegistration

    module Storage = 
        open System

        let finish =
            finishFunc

    let setStorage (service : StorageService) =
        finishFunc <- service.finishRegistration
