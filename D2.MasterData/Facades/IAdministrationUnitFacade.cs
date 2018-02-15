using D2.MasterData.Parameters;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace D2.MasterData.Facades
{
    public interface IAdministrationUnitFacade
    {
        void CreateNewAdministrationUnit(AdministrationUnitParameters value);
    }
}
