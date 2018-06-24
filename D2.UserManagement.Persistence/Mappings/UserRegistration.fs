namespace D2.UserManagement.Persistence

open D2.UserManagement.Persistence
open FluentNHibernate.Mapping
open NHibernate.Type

type UserRegistrationCreateMap() =
    inherit ClassMap<UserRegistrationI>()
    do
        base.Table("registrations") 
        base.Id(fun x -> x.Id :> obj) |> ignore
        base.Map(fun x -> x.FirstName :> obj)
            .Length(255)
            .Column("first_name")
            .Nullable()
            |> ignore
        base.Map(fun x -> x.LastName :> obj)
            .Length(255)
            .Column("last_name")
            .Not.Nullable()
            |> ignore
        base.Map(fun x -> x.Salutation :> obj)
            .Length(50)
            .Nullable()
            |> ignore
        base.Map(fun x -> x.Title :> obj)
            .Length(50)
            .Nullable()
            |> ignore
        base.Map(fun x -> x.EMail :> obj)
            .Length(255)
            .Not.Nullable()
            |> ignore
        base.Map(fun x -> x.Login :> obj)
            .Length(255)
            .Not.Nullable()
            |> ignore
        base.Map(fun x -> x.MailSent :> obj)
            .CustomType<UtcDateTimeType>()
            .Column("mail_sent")
            .Nullable()
            |> ignore

type UserRegistrationMap() =
    inherit UserRegistrationCreateMap()
    do
        base.Version(fun x -> x.Version :> obj)
            .Column("xmin")
            .Generated.Always()
            |> ignore
