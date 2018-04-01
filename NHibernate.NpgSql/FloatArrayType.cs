using System.Data;
using NpgsqlTypes;

namespace Beginor.NHibernate.NpgSql {

    public class FloatArrayType : ArrayType<float> {

        protected override NpgSqlType GetNpgSqlType() => new NpgSqlType(
            DbType.Object,
            NpgsqlDbType.Array | NpgsqlDbType.Real
        );

    }

}
