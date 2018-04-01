using System.Data;
using NpgsqlTypes;

namespace Beginor.NHibernate.NpgSql {
    
    public class Int16ArrayType : ArrayType<short> {

        protected override NpgSqlType GetNpgSqlType() => new NpgSqlType(
            DbType.Object,
            NpgsqlDbType.Array | NpgsqlDbType.Smallint
        );

    }

}
