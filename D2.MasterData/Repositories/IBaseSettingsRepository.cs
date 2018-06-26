using D2.MasterData.Models;
using System;
using System.Collections.Generic;

namespace D2.MasterData.Repositories
{
    public interface IBaseSettingsRepository
    {
        void Insert(AdministrationUnitsFeature item);
        IEnumerable<AdministrationUnitsFeature> List();
        AdministrationUnitsFeature Load(Guid id);
        void Update(AdministrationUnitsFeature administrationUnitFeature);
    }
}
