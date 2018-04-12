using D2.MasterData.Infrastructure;
using D2.MasterData.Models;
using D2.Service.IoC;
using Microsoft.EntityFrameworkCore;
using System;
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
            var result = from unit in _connection.AdministrationUnits
                                                 .Include(unit => unit.Entrances)
                                                 .Include("Entrances.Address")
                                                 .Include("Entrances.Address.Country")
                         orderby unit.UserKey
                         select unit;

            return result.ToList();
        }

        public AdministrationUnit Load(Guid id)
        {
            var result = from unit in _connection.AdministrationUnits
                                                 .Include(unit => unit.Entrances)
                                                 .Include("Entrances.Address")
                                                 .Include("Entrances.Address.Country")
                         where unit.Id == id
                         select unit;

            return result.SingleOrDefault();
        }

        public void Update(AdministrationUnit administrationUnit)
        {
            _connection.Update(administrationUnit);
            _connection.SaveChanges();
        }
    }
}
