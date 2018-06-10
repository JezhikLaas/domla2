namespace D2.Welcome.Types

open System

type FinishRegistration () =

    member val id = String.Empty with get, set
    
    member val password = String.Empty with get, set