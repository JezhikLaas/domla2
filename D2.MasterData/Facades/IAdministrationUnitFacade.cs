using D2.MasterData.Models;
using D2.MasterData.Parameters;
using D2.Service.Contracts.Execution;
using D2.Service.Contracts.Validation;
using System;
using System.Collections.Generic;

namespace D2.MasterData.Facades
{
    public interface IAdministrationUnitFacade
    {
        Guid CreateNewAdministrationUnit(AdministrationUnitParameters value);
        void EditAdministrationUnit(AdministrationUnitParameters value);
        IEnumerable<AdministrationUnit> ListAdministrationUnits();
        ExecutionResponse LoadAdministrationUnit(string id);
        ValidationResponse ValidateCreate(AdministrationUnitParameters value);
        ValidationResponse ValidateEdit(AdministrationUnitParameters value);
        void AddAdministrationUnitProperty(SelectedAdministrationUnitPropertyParameters value);
        ValidationResponse ValidateAddProperty(SelectedAdministrationUnitPropertyParameters value);
    }
}
