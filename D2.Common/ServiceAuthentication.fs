namespace D2.Common

[<AutoOpen>]
module ServiceAuthentication =
    open Microsoft.AspNetCore.Builder
    open Microsoft.Extensions.DependencyInjection
    open System

    type IServiceCollection with
        member this.AddServiceAuthentication() =
            this
                .AddMemoryCache()
                .AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(
                    "Bearer",
                    fun options -> options.Authority <- (ServiceConfiguration.authority.StandardAddress)
                                   options.CacheDuration <- TimeSpan.FromSeconds 10.0
                                   options.ApiName <- ServiceConfiguration.authority.ClientId
                                   options.ApiSecret <- ServiceConfiguration.authority.ClientSecret
                                   options.EnableCaching <- true
                                   options.RequireHttpsMetadata <- false
                                   options.SaveToken <- true
                )