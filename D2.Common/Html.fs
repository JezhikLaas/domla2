[<AutoOpen>]
module StringExtensions
    open System
    open System.Net

    type String with
        member this.Html() =
            WebUtility.HtmlEncode (this)
