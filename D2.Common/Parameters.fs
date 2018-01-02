[<AutoOpen>]
module Parameters
    
    let inline (<<) (p:Npgsql.NpgsqlParameterCollection) (v:(string * obj)) =
        p.AddWithValue(fst v, snd v) |> ignore
        p

    let inline (<<<) (p:Npgsql.NpgsqlParameterCollection) (v:(string * NpgsqlTypes.NpgsqlDbType * obj)) =
        let name, dbType, value = v
        p.AddWithValue(name, dbType, value) |> ignore
        p