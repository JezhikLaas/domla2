using D2.MasterData.Models;
using D2.MasterData.Parameters;
using System.Collections.Generic;

namespace D2.MasterData.Facades
{
    public interface IAdministrationUnitFacade
    {
        void CreateNewAdministrationUnit(AdministrationUnitParameters value);
        IEnumerable<AdministrationUnit> ListAdministrationUnits();
    }
}
