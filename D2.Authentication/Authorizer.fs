namespace D2.Authentication

open IdentityServer4.ResponseHandling
open IdentityServer4.Validation
open IdentityServer4.Models
open Microsoft.AspNetCore.Authentication
open Microsoft.Extensions.Logging
open IdentityServer4.Services
open System.Threading.Tasks

type Authorizer
     (
         clock : ISystemClock,
         logger : ILogger<AuthorizeInteractionResponseGenerator>,
         consent : IConsentService,
         profile : IProfileService

     ) =
    
    member this.ProcessInteractionAsync (request : ValidatedAuthorizeRequest, consent : ConsentResponse) =
        ()