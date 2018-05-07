using System.Data;
using System.Data.Common;
using FluentNHibernate.Cfg.Db;
using NHibernate.Dialect;
using NHibernate.Engine;
using NHibernate.Driver;

namespace D2.MasterData.Test.Helper
{
    public class MicrosoftSqliteDriver : ReflectionBasedDriver
    {
        public MicrosoftSqliteDriver() : base(
            "Microsoft.Data.Sqlite",
            "Microsoft.Data.Sqlite",
            "Microsoft.Data.Sqlite.SqliteConnection",
            "Microsoft.Data.Sqlite.SqliteCommand")
        {
        }

        public override DbConnection CreateConnection()
        {
            var connection = base.CreateConnection();
            connection.StateChange += Connection_StateChange;
            return connection;
        }

        private static void Connection_StateChange(object sender, StateChangeEventArgs e)
        {
            if ((e.OriginalState == ConnectionState.Broken || e.OriginalState == ConnectionState.Closed || e.OriginalState == ConnectionState.Connecting) &&
                e.CurrentState == ConnectionState.Open)
            {
                var connection = (DbConnection)sender;
                using (var command = connection.CreateCommand())
                {
                    // Activated foreign keys if supported by SQLite.  Unknown pragmas are ignored.
                    command.CommandText = "PRAGMA foreign_keys = ON";
                    command.ExecuteNonQuery();
                }
            }
        }

        public override IResultSetsCommand GetResultSetsCommand(ISessionImplementor session)
        {
            return new BasicResultSetsCommand(session);
        }

        public override bool UseNamedPrefixInSql => true;

        public override bool UseNamedPrefixInParameter => true;

        public override string NamedPrefix => "@";

        public override bool SupportsMultipleOpenReaders => false;

        public override bool SupportsMultipleQueries => true;

        public override bool SupportsNullEnlistment => false;

        public override bool HasDelayedDistributedTransactionCompletion => true;
    }
    
    public class SqliteConfiguration : PersistenceConfiguration<SqliteConfiguration>
    {
        public static SqliteConfiguration Standard => new SqliteConfiguration();

        public SqliteConfiguration()
        {
            Driver<MicrosoftSqliteDriver>();
            Dialect<SQLiteDialect>();
            Raw("query.substitutions", "true=1;false=0");
        }

        public SqliteConfiguration InMemory()
        {
            Raw("connection.release_mode", "on_close");
            Raw("use_proxy_validator", "false");
            return ConnectionString(c => c.Is("Data Source=:memory:"));
        }

        public SqliteConfiguration UsingFile(string fileName)
        {
            Raw("use_proxy_validator", "false");
            return ConnectionString(c => c.Is(string.Format("Data Source={0}", fileName)));
        }
    }
}