using D2.MasterData.Controllers.Validators;
using D2.MasterData.Models;
using D2.MasterData.Parameters;
using D2.MasterData.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace D2.MasterData.Facades.Implementation
{
    public class AdministrationUnitFacade : IAdministrationUnitFacade
    {
        private IAdministrationUnitRepository _repository;

        public AdministrationUnitFacade(
            IAdministrationUnitRepository repository)
        {
            _repository = repository;
        }

        public ValidationResult CreateNewAdministrationUnit(AdministrationUnitPostParameters value)
        {
            var result = value.Validate();
            if (result.IsValid) _repository.Insert(new AdministrationUnit(value));

            return result;
        }
    }
}
