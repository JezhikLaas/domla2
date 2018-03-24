namespace D2.ServiceBroker.Persistence

open D2.Common
open System

type EndPoint =

    abstract member Name : string

    abstract member Uri : string

type Service =

    abstract member Name : string

    abstract member BaseUrl : string

    abstract member Group : string

    abstract member Version : int

    abstract member Patch : int

    abstract member EndPoints : EndPoint list

type Application =

    abstract member Id : Guid

    abstract member Name : string

    abstract member Version : int

    abstract member Patch : int

    abstract member Services : Service list

type StorageService = {
    services : string -> int -> Async<Service list>
    applications : Async<Application list>
    register : string * int -> Service -> Async<unit>
    endpoints : string * int -> string -> Async<Service option>
}
