using D2.MasterData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace D2.MasterData.Repositories
{
    public interface IAdministrationUnitPropertyRepository
    {
        IEnumerable<AdministrationUnitProperty> List();
        AdministrationUnitProperty Load(Guid id);
        void Update(AdministrationUnitProperty administrationUnitProperty);
    }
}
