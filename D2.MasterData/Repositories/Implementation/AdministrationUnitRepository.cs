using D2.MasterData.Models;
using Dapper.FastCrud;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace D2.MasterData.Repositories.Implementation
{
    public class AdministrationUnitRepository : IAdministrationUnitRepository
    {
        IDbConnection _connection;

        public AdministrationUnitRepository(IDbConnection connection)
        {
            OrmConfiguration.DefaultDialect = SqlDialect.PostgreSql;
            _connection = connection;
        }

        public void Insert(AdministrationUnit item)
        {
            _connection.Insert(item);
        }
    }
}
