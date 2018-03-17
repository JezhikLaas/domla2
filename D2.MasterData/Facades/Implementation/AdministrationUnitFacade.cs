using D2.MasterData.Models;
using D2.MasterData.Parameters;
using D2.MasterData.Repositories;
using D2.Service.IoC;
using System.Collections.Generic;

namespace D2.MasterData.Facades.Implementation
{
    [RequestScope]
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

        public IEnumerable<AdministrationUnit> ListAdministrationUnits()
        {
            return _repository.List();
        }
    }
}
