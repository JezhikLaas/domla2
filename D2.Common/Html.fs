[<AutoOpen>]
module StringExtensions
    open System
    open System.Net

    type String with
        member this.Html() =
            WebUtility.HtmlEncode (this)
        
        member this.Base64UrlEncode() =
            if this = null then
                ""
            else
                let result = this
                             |>
                             System.Text.Encoding.UTF8.GetBytes
                             |>
                             Convert.ToBase64String
                             |>
                             String.map(
                                 fun c -> match c with
                                 | '+' -> '-'
                                 | '/' -> '_'
                                 | _   -> c
                            )
                result.Replace("=", "%3d")
                              

