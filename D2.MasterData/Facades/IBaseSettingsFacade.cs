using D2.MasterData.Models;
using D2.MasterData.Parameters;
using D2.Service.Contracts.Execution;
using D2.Service.Contracts.Validation;
using System.Collections.Generic;

namespace D2.MasterData.Facades
{
    public interface IBaseSettingsFacade
    {
        void CreateNewAdministrationUnitsFeature(AdministrationUnitsFeatureParameters value);
        void EditAdministrationUnitsFeature(AdministrationUnitsFeatureParameters value);
        IEnumerable<AdministrationUnitsFeature> ListAdministrationUnitsFeatures();
        ExecutionResponse LoadAdministrationUnitsFeature(string id);
        ValidationResponse ValidateCreate(AdministrationUnitsFeatureParameters value);
        ValidationResponse ValidateEdit(AdministrationUnitsFeatureParameters value);
    }
}
