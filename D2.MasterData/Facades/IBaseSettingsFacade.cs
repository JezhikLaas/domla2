using D2.MasterData.Models;
using D2.MasterData.Parameters;
using D2.Service.Contracts.Execution;
using D2.Service.Contracts.Validation;
using System.Collections.Generic;

namespace D2.MasterData.Facades
{
    interface IBasicSettingsFacade
    {
        void CreateNewAdministrationUnitFeature(AdministrationUnitFeatureParameters value);
        void EditAdministrationUnitFeature(AdministrationUnitFeatureParameters value);
        IEnumerable<AdministrationUnitFeature> ListAdministrationUnitFeatures();
        ExecutionResponse LoadAdministrationUnitFeature(string id);
        ValidationResponse ValidateCreate(AdministrationUnitFeatureParameters value);
        ValidationResponse ValidateEdit(AdministrationUnitFeatureParameters value);
    }
}
