namespace D2.ServiceBroker

open D2.ServiceBroker.Persistence

module CompositionRoot =

    let mutable private versionsFunc =
        Applications.Storage.applications

    let mutable private servicesFunc =
        Applications.Storage.services

    let mutable private registerFunc =
        Applications.Storage.register

    let mutable private endpointsFunc =
        Applications.Storage.endpoints

    module Storage = 

        let applications () =
            versionsFunc
        
        let routes (appName : string) (appVersion : int) =
            servicesFunc appName appVersion
        
        let register (appName : string) (appVersion : int) (service: Service) =
            registerFunc (appName, appVersion) service
        
        let endpoints (appName : string) (appVersion : int) (service: string) =
            endpointsFunc (appName, appVersion) service

    let setStorage (service : StorageService) =
        versionsFunc  <- service.applications
        servicesFunc  <- service.services
        registerFunc  <- service.register
        endpointsFunc <- service.endpoints