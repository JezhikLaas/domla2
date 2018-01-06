namespace D2.Authentication

open System
open System.ComponentModel.DataAnnotations

type LoginInputModel () =
    
    [<Required>]
    member val Username = String.Empty with get, set
    
    [<Required>]
    member val Password = String.Empty with get, set

    member val ReturnUrl = String.Empty with get, set