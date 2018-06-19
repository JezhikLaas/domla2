using D2.MasterData.Models;
using System;
using System.Collections.Generic;

namespace D2.MasterData.Repositories
{
    public interface IBasicSettingsRepository
    {
        void Insert(AdministrationUnitFeature item);
        IEnumerable<AdministrationUnitFeature> List();
        AdministrationUnitFeature Load(Guid id);
        void Update(AdministrationUnitFeature administrationUnitFeature);
    }
}
