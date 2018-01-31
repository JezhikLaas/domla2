namespace D2.Common

[<AutoOpen>]
module InteractiveAuthentication =
    open Microsoft.AspNetCore.Authentication
    open Microsoft.AspNetCore.Authentication.OpenIdConnect
    open Microsoft.Extensions.DependencyInjection

    type IServiceCollection with
        member this.AddInteractiveAuthentication() =
            this
                .AddAuthentication(fun options ->
                    options.DefaultScheme <- "Cookies"
                    options.DefaultChallengeScheme <- "oidc"
                )
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", fun options -> 
                    options.SignInScheme <- "Cookies"

                    options.Authority <- (ServiceConfiguration.authority.FullAddress)
                    options.RequireHttpsMetadata <- false

                    options.ClientId <- "interactive"
                    options.ClientSecret <- "0A0C7C53-1A60-4D5D-AE4C-4163F72E467D"
                    options.ResponseType <- "code id_token"

                    options.SaveTokens <- true
                    options.GetClaimsFromUserInfoEndpoint <- true

                    options.Scope.Add("offline_access")
                    options.Scope.Add("profile")
                    options.Scope.Add("role.profile")
                    options.Scope.Add("api")
                    options.TokenValidationParameters.NameClaimType <- "name"
                    options.TokenValidationParameters.RoleClaimType <- "role"
                )   
