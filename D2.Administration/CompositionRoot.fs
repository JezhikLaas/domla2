namespace D2.Administration

open D2.UserManagement.Persistence
open System

module CompositionRoot =

    let mutable private registerFunc =
        Users.Storage.register

    let mutable private pendingFunc =
        Users.Storage.listPending

    let mutable private acceptRegistrationFunc =
        Users.Storage.acceptRegistration

    module Storage = 

        let register (user : UserRegistration) =
            registerFunc user

        let listPending () =
            pendingFunc
        
        let acceptRegistration (id : Guid) (prerequisite : (UserRegistration -> Async<bool>)) =
            acceptRegistrationFunc id prerequisite

    let setStorage (service : StorageService) =
        registerFunc <- service.register
        pendingFunc  <- service.listPending
        acceptRegistrationFunc <- service.acceptRegistration