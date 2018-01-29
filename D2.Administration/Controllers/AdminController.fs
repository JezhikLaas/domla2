namespace D2.Administration.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore.Mvc

[<Route("[controller]")>]
type AdminController () =
    inherit Controller()
