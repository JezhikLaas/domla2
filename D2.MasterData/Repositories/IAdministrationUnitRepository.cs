using D2.MasterData.Models;
using System;
using System.Collections.Generic;

namespace D2.MasterData.Repositories
{
    public interface IAdministrationUnitRepository
    {
        void Insert(AdministrationUnit item);
        IEnumerable<AdministrationUnit> List();
        AdministrationUnit Load(Guid id);
    }
}
