namespace D2.Common

type FSharpFuncUtil<'T, 'TResult> = 

    static member ToFSharpFunc<'a,'b> (func:System.Converter<'a,'b>) = fun x -> func.Invoke(x)

    static member ToFSharpFunc<'a,'b> (func:System.Func<'a,'b>) = fun x -> func.Invoke(x)

    static member ToFSharpFunc<'a,'b,'c> (func:System.Func<'a,'b,'c>) = fun x y -> func.Invoke(x,y)

    static member ToFSharpFunc<'a,'b,'c,'d> (func:System.Func<'a,'b,'c,'d>) = fun x y z -> func.Invoke(x,y,z)
