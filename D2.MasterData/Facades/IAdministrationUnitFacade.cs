using D2.MasterData.Controllers.Validators;
using D2.MasterData.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace D2.MasterData.Facades
{
    public interface IAdministrationUnitFacade
    {
        ValidationResult CreateNewAdministrationUnit(AdministrationUnitPostParameters value);
    }
}
