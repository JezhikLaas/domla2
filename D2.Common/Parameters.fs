[<AutoOpen>]
module Parameters

    open System

    type StorageData =
    | IntField of int32
    | LongField of int64
    | StringField of string
    | JsonField of string
    | GuidField of Guid
    | TimeStampField of DateTime


    let inline (<<) (p:Npgsql.NpgsqlParameterCollection) (v:(string * StorageData)) =
        let dbInfo = match snd v with
                     | StorageData.IntField       value -> (NpgsqlTypes.NpgsqlDbType.Integer,   value :> obj)
                     | StorageData.LongField      value -> (NpgsqlTypes.NpgsqlDbType.Bigint,    value :> obj)
                     | StorageData.GuidField      value -> (NpgsqlTypes.NpgsqlDbType.Uuid,      value :> obj)
                     | StorageData.StringField    value -> (NpgsqlTypes.NpgsqlDbType.Varchar,   value :> obj)
                     | StorageData.JsonField      value -> (NpgsqlTypes.NpgsqlDbType.Jsonb,     value :> obj)
                     | StorageData.TimeStampField value -> (NpgsqlTypes.NpgsqlDbType.Timestamp, value :> obj)
        p.AddWithValue(fst v, fst dbInfo, snd dbInfo) |> ignore
        p
