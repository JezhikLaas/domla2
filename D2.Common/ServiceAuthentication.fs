namespace D2.Common

[<AutoOpen>]
module ServiceAuthentication =
    open Microsoft.Extensions.DependencyInjection
    open System

    type IServiceCollection with
        member this.AddServiceAuthentication() =
            this
                .AddMemoryCache()
                .AddAuthentication("Bearer")
                .AddOAuth2Introspection(
                    "Bearer",
                    fun options -> options.Authority <- (ServiceConfiguration.authority.FullAddress)
                                   options.CacheDuration <- TimeSpan.FromSeconds 10.0
                                   options.ClientId <- ServiceConfiguration.authority.ClientId
                                   options.ClientSecret <- ServiceConfiguration.authority.ClientSecret
                                   options.EnableCaching <- true
                                   options.DiscoveryPolicy.RequireHttps <- false
                                   options.SaveToken <- true
                )