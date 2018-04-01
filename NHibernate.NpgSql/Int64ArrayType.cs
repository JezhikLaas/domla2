﻿using System.Data;
using NpgsqlTypes;

namespace Beginor.NHibernate.NpgSql {

    public class Int64ArrayType : ArrayType<long> {

        protected override NpgSqlType GetNpgSqlType() => new NpgSqlType(
            DbType.Object,
            NpgsqlDbType.Array | NpgsqlDbType.Bigint
        );

    }

}
