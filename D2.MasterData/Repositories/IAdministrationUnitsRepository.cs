using D2.MasterData.Models;
using System;
using System.Collections.Generic;

namespace D2.MasterData.Repositories
{
    public interface IAdministrationUnitsRepository
    {
        void Insert(AdministrationUnit item);
        IEnumerable<AdministrationUnit> List();
        AdministrationUnit Load(Guid id);
        void Update(AdministrationUnit administrationUnit);
    }
}
