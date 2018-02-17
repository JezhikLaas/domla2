using D2.MasterData.Models;
using D2.MasterData.Parameters;
using D2.MasterData.Repositories;
using FluentValidation.Results;
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

        public void CreateNewAdministrationUnit(AdministrationUnitParameters value)
        {
            var administrationUnit = new AdministrationUnit(value);
            _repository.Insert(administrationUnit);
        }
    }
}
