namespace D2.Welcome

open D2.UserManagement.Persistence

module CompositionRoot =

    let mutable private finishFunc =
        Users.Storage.finishRegistration

    let mutable private createDbFunc =
        Users.Storage.createDatabase

    module Storage = 
        open System

        let finish =
            finishFunc

        let createDatabase =
            createDbFunc

    let setStorage (service : StorageService) =
        finishFunc <- service.finishRegistration
        createDbFunc <- service.createDatabase
