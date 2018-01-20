namespace D2.Authentication

open System
open IdentityServer4.Models

type AuthorizeResultModel () =

    member val RedirectUri = String.Empty with get, set

    member val State = String.Empty with get, set

    member val Scope = String.Empty with get, set

    member val IdentityToken = String.Empty with get, set

    member val AccessToken = String.Empty with get, set

    member val AccessTokenLifetime = 0 with get, set

    member val Code = String.Empty with get, set

    member val SessionState = String.Empty with get, set

    member val Error = String.Empty with get, set

    member val ErrorDescritpion = String.Empty with get, set

    member val IsError = false with get, set

    new (data : IdentityServer4.ResponseHandling.AuthorizeResponse) as this =
        AuthorizeResultModel () then
            this.RedirectUri <- data.RedirectUri
            this.State <- data.State
            this.Scope <- data.Scope
            this.IdentityToken <- data.IdentityToken
            this.AccessToken <- data.AccessToken
            this.AccessTokenLifetime <- data.AccessTokenLifetime
            this.Code <- data.Code
            this.SessionState <- data.SessionState
            this.Error <- data.Error
            this.ErrorDescritpion <- data.ErrorDescription
            this.IsError <- data.IsError
