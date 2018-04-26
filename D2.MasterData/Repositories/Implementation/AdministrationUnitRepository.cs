using D2.MasterData.Infrastructure;
using D2.MasterData.Models;
using D2.Service.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;

namespace D2.MasterData.Repositories.Implementation
{
    [RequestScope]
    public class AdministrationUnitRepository : IAdministrationUnitRepository
    {
        readonly IDataContext _context;

        public AdministrationUnitRepository(IDataContext context)
        {
            _context = context;
        }

        public void Insert(AdministrationUnit item)
        {
            using (var transaction = _context.Session.BeginTransaction())
            {
                _context.Session.Save(item);
                transaction.Commit();
            }
        }

        public IEnumerable<AdministrationUnit> List()
        {
            var result = from unit in _context.Session.Query<AdministrationUnit>()
                         orderby unit.UserKey
                         select unit;

            return result.ToList();
        }

        public AdministrationUnit Load(Guid id)
        {
            return _context.Session.Get<AdministrationUnit>(id);
        }

        public void Update(AdministrationUnit administrationUnit)
        {
            using (var transaction = _context.Session.BeginTransaction())
            {
                _context.Session.Update(administrationUnit);
                transaction.Commit();
            }
        }
    }
}
