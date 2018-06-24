namespace D2.UserManagement.Persistence

open D2.UserManagement.Persistence
open FluentNHibernate.Mapping
open NHibernate.Type

type UserCreateMap() =
    inherit ClassMap<UserI>()
    do
        base.Table("users")
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
        base.Map(fun x -> x.Password :> obj)
            .Length(255)
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
        base.Map(fun x -> x.Claims :> obj)
            .CustomType<Beginor.NHibernate.NpgSql.JsonbType>()
            .Nullable()
            |> ignore
        base.Map(fun x -> x.LoggedIn :> obj)
            .CustomType<UtcDateTimeType>()
            .Column("logged_in")
            .Nullable()
            |> ignore
        base.Map(fun x -> x.PrivacyAccepted :> obj)
            .CustomType<UtcDateTimeType>()
            .Column("privacy_accepted")
            .Nullable()
            |> ignore

type UserMap() =
    inherit UserCreateMap()
    do
        base.Version(fun x -> x.Version :> obj)
            .Column("xmin")
            .Generated.Always()
            |> ignore
        