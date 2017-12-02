[<AutoOpen>]
module Cast
    let inline (!>) (x:^a) : ^b = ((^a or ^b) : (static member op_Implicit : ^a -> ^b) x)

    let inline (!?) (x:^a) (y:^a) : ^a = if x |> isNull then y else x
