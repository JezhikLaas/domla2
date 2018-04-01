using System.Data;
using NpgsqlTypes;

namespace Beginor.NHibernate.NpgSql {

    public class StringArrayType : ArrayType<string> {

        protected override NpgSqlType GetNpgSqlType() {
            return new NpgSqlType(
                DbType.Object,
                NpgsqlDbType.Array | NpgsqlDbType.Text
            );
        }

    }

}
