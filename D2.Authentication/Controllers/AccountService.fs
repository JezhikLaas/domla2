namespace D2.Authentication

open D2.Common
open IdentityModel
open IdentityServer4.Extensions
open IdentityServer4.Services
open IdentityServer4.Stores
open Microsoft.AspNetCore.Authentication
open Microsoft.AspNetCore.Http
open System
open System.Linq
open System.Threading.Tasks

type AccountService
     (
        interaction : IIdentityServerInteractionService,
        clientStore : IClientStore,
        httpContextAccessor : IHttpContextAccessor
     )
     =

     let logger = Logger.get "D2.Authentication.Controllers.AccountService"

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

