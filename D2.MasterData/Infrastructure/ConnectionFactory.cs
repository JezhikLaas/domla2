using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using D2.Common;
using D2.MasterData.Models;
using D2.MasterData.Parameters;
using Dapper.FastCrud;
using Npgsql;

namespace D2.MasterData.Infrastructure
{
    public class ConnectionFactory
    {
        public static IDbConnection CreateConnection()
        {
            var configuration = ServiceConfiguration.connectionInfo;
            var builder = new NpgsqlConnectionStringBuilder();
            builder.ApplicationName = configuration.Identifier;
            builder.Database = configuration.Name;
            builder.Host = configuration.Host;
            builder.Password = configuration.Password;
            builder.Username = configuration.User;
            builder.Port = configuration.Port;

            var result = new NpgsqlConnection(builder.ConnectionString);
            result.Open();
            return result;
        }
    }
}
