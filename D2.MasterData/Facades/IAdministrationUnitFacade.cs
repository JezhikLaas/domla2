using D2.MasterData.Models;
using D2.MasterData.Parameters;
using D2.Service.Contracts.Execution;
using D2.Service.Contracts.Validation;
using System.Collections.Generic;

namespace D2.MasterData.Facades
{
    public interface IAdministrationUnitFacade
    {
        void CreateNewAdministrationUnit(AdministrationUnitParameters value);
        IEnumerable<AdministrationUnit> ListAdministrationUnits();
        ExecutionResponse LoadAdministrationUnit(string id);
        ValidationResponse ValidateCreate(AdministrationUnitParameters value);
    }
}
