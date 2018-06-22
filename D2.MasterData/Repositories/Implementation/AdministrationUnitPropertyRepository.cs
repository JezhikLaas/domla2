using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using D2.MasterData.Infrastructure;
using D2.MasterData.Models;
using D2.Service.IoC;

namespace D2.MasterData.Repositories.Implementation
{
    [RequestScope]
    public class AdministrationUnitPropertyRepository : IAdministrationUnitPropertyRepository
    {
        readonly IDataContext _context;

        public AdministrationUnitPropertyRepository (IDataContext context)
        {
            _context = context;
        }

        public IEnumerable<AdministrationUnitProperty> List()
        {
            var result = from unit in _context.Session.Query<AdministrationUnitProperty>()
                         orderby unit.Title
                         select unit;

            return result.ToList();
        }

        public AdministrationUnitProperty Load(Guid id)
        {
            return _context.Session.Get<AdministrationUnitProperty>(id);
        }

        public void Update(AdministrationUnitProperty administrationUnitFeature)
        {
            using (var transaction = _context.Session.BeginTransaction())
            {
                _context.Session.Update(administrationUnitFeature);
                transaction.Commit();
            }
        }
    }
}
