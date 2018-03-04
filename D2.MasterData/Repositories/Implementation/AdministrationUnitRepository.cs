using D2.MasterData.Infrastructure;
using D2.MasterData.Infrastructure.IoC;
using D2.MasterData.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace D2.MasterData.Repositories.Implementation
{
    [RequestScope]
    public class AdministrationUnitRepository : IAdministrationUnitRepository
    {
        MasterDataContext _connection;

        public AdministrationUnitRepository(MasterDataContext context)
        {
            _connection = context;
        }

        public void Insert(AdministrationUnit item)
        {
            _connection.Add(item);
            _connection.SaveChanges();
        }

        public IEnumerable<AdministrationUnit> List()
        {
            var result = from unit in _connection.AdministrationUnits.Include(unit => unit.Entrances)
                         orderby unit.UserKey
                         select unit;
            return result.ToList();
        }
    }
}
