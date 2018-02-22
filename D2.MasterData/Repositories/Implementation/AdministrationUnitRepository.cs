using D2.MasterData.Infrastructure;
using D2.MasterData.Models;
using D2.MasterData.Parameters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace D2.MasterData.Repositories.Implementation
{
    public class AdministrationUnitRepository : IAdministrationUnitRepository
    {
        MasterDataContext _connection;

        public AdministrationUnitRepository(MasterDataContext context)
        {
            _connection = context;
        }

        public void Insert(AdministrationUnit item)
        {
            item.Id = Guid.NewGuid();
            _connection.Add(item);
            _connection.SaveChanges();
        }

        public IEnumerable<AdministrationUnit> List()
        {
            var result = _connection
                .AdministrationUnits
                .OrderBy(unit => unit.UserKey)
                .ToList();
            return result;
        }
    }
}
