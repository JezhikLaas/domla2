namespace D2.Authentication

module Storage =

    let storages = {
        setupStorage = SetupData.access;
        persistedGrantStorage = PersistedGrantData.access;
        resourceStorage = ResourceData.access;
        clientStorage = ClientData.access;
        userStorage = UserData.access;
        authorizationStorage = AuthorizationCodeData.access
    }
