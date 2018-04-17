namespace D2.Common

[<AutoOpen>]
module ServiceAuthentication =
    open Microsoft.Extensions.DependencyInjection
    open System

    type IServiceCollection with
        member this.AddServiceAuthentication() =
            this
                .AddAuthentication("Bearer")
                .AddOAuth2Introspection(
                    fun options -> options.Authority <- (ServiceConfiguration.authority.FullAddress)
                                   options.EnableCaching <- true
                                   options.ClientId <- "api"
                                   options.ClientSecret <- "78C2A2A1-6167-45E4-A9D7-46C5D921F7D5"
                                   options.SaveToken <- true
                                   options.CacheDuration <- TimeSpan.FromMinutes 10.0
                )