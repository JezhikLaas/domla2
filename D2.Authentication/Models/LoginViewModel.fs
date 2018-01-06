namespace D2.Authentication

open System

type LoginViewModel () =
    inherit LoginInputModel ()

    member val AllowRememberLogin = false with get, set

    member val EnableLocalLogin = false with get, set

