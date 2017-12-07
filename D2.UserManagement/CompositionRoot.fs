namespace D2.UserManagement

open D2.UserManagement.Persistence

module CompositionRoot =

    let mutable private registerFunc =
        Users.Storage.register

    let mutable private pendingFunc =
        Users.Storage.listPending

    module Storage = 

        let register (user : UserRegistration) =
            registerFunc user

        let listPending () =
            pendingFunc

    let setStorage (service : StorageService) =
        registerFunc <- service.register
        pendingFunc  <- service.listPending