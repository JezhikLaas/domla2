using D2.MasterData.Models;
using System;
using System.Collections.Generic;

namespace D2.MasterData.Repositories
{
    public interface IBasicSettingsRepository
    {
        void Insert(AdministrationUnitsFeature item);
        IEnumerable<AdministrationUnitsFeature> List();
        AdministrationUnitsFeature Load(Guid id);
        void Update(AdministrationUnitsFeature administrationUnitFeature);
    }
}
