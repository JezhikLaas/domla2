using D2.MasterData.Infrastructure;
using D2.MasterData.Models;
using D2.Service.IoC;
using System;
using System.Collections.Generic;
using System.Linq;

namespace D2.MasterData.Repositories.Implementation
{
    [RequestScope]
    public class BaseSettingsRepository : IBaseSettingsRepository
    {
        readonly IDataContext _context;

        public BaseSettingsRepository(IDataContext context)
        {
            _context = context;
        }

        public void Insert(AdministrationUnitsFeature item)
        {
            using (var transaction = _context.Session.BeginTransaction())
            {
                _context.Session.Save(item);
                transaction.Commit();
            }
        }

        public IEnumerable<AdministrationUnitsFeature> List()
        {
            var result = from unit in _context.Session.Query<AdministrationUnitsFeature>()
                         orderby unit.Title
                         select unit;

            return result.ToList();
        }

        public AdministrationUnitsFeature Load(Guid id)
        {
            return _context.Session.Get<AdministrationUnitsFeature>(id);
        }

        public void Update(AdministrationUnitsFeature administrationUnitFeature)
        {
            using (var transaction = _context.Session.BeginTransaction())
            {
                _context.Session.Update(administrationUnitFeature);
                transaction.Commit();
            }
        }
    }
}
