namespace D2.Authentication

open IdentityServer4.Services
open IdentityServer4.Stores
open Microsoft.AspNetCore.Http

type AccountService
     (
        interaction : IIdentityServerInteractionService,
        clientStore : IClientStore,
        httpContextAccessor : IHttpContextAccessor
     )
     =

     member this.BuildLoginViewModel (returnUrl : string) =
         let context = interaction.GetAuthorizationContextAsync (returnUrl)
                       |> Async.AwaitTask
                       |> Async.RunSynchronously
         
         LoginViewModel (
             AllowRememberLogin = false,
             EnableLocalLogin = false,
             ReturnUrl = returnUrl,
             Username = (if context = null then null else context.LoginHint)
         )

