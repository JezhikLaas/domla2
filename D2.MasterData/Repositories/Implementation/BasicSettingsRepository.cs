using D2.MasterData.Infrastructure;
using D2.MasterData.Models;
using D2.Service.IoC;
using System;
using System.Collections.Generic;
using System.Linq;

namespace D2.MasterData.Repositories.Implementation
{
    [RequestScope]
    public class BasicSettingsRepository : IBasicSettingsRepository
    {
        readonly IDataContext _context;

        public BasicSettingsRepository(IDataContext context)
        {
            _context = context;
        }

        public void Insert(AdministrationUnitFeature item)
        {
            using (var transaction = _context.Session.BeginTransaction())
            {
                _context.Session.Save(item);
                transaction.Commit();
            }
        }

        public IEnumerable<AdministrationUnitFeature> List()
        {
            var result = from unit in _context.Session.Query<AdministrationUnitFeature>()
                         orderby unit.Title
                         select unit;

            return result.ToList();
        }

        public AdministrationUnitFeature Load(Guid id)
        {
            return _context.Session.Get<AdministrationUnitFeature>(id);
        }

        public void Update(AdministrationUnitFeature administrationUnitFeature)
        {
            using (var transaction = _context.Session.BeginTransaction())
            {
                _context.Session.Update(administrationUnitFeature);
                transaction.Commit();
            }
        }
    }
}
