using System.Data;
using NpgsqlTypes;

namespace Beginor.NHibernate.NpgSql {

    public class BooleanArrayType : ArrayType<bool> {

        protected override NpgSqlType GetNpgSqlType() => new NpgSqlType(
            DbType.Object,
            NpgsqlDbType.Array | NpgsqlDbType.Boolean
        );

    }

}
