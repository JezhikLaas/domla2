[<AutoOpen>]
module StreamExtensions
    open System.IO
    open System.Text
    
    type Stream with
        member this.AsUtf8String() =
            use stream = new MemoryStream()
            this.CopyTo(stream)
            stream.Capacity <- int stream.Length
            Encoding.UTF8.GetString(stream.GetBuffer())
