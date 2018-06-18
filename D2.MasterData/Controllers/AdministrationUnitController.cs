using D2.MasterData.Facades;
using D2.MasterData.Infrastructure;
using D2.MasterData.Parameters;
using D2.Service.Contracts.Common;
using D2.Service.Contracts.Execution;
using D2.Service.Contracts.Validation;
using D2.Service.Controller;
using Microsoft.AspNetCore.Http;

namespace D2.MasterData.Controllers
{
    /// <summary>
    /// Instanz dieser Klasse und alle Instanzen darunterliegender Klassen werden
    /// von Dependency Injection automatisch erzeugt
    /// </summary>
    public class AdministrationUnitController : BaseController
    {
        IAdministrationUnitFacade _administrationUnitFacade;
        IPostalCodeInfoFacade _postalCodeInfoFacade;

        /// <summary>
        /// Konstruktor.
        /// </summary>
        /// <param name="administrationUnitFacade">Fassade für fachliche Aufgaben.</param>
        public AdministrationUnitController(
            IAdministrationUnitFacade administrationUnitFacade, IPostalCodeInfoFacade postalCodeInfoFacade)
        {
            _administrationUnitFacade = administrationUnitFacade;
            _postalCodeInfoFacade = postalCodeInfoFacade;
        }

        [Routing("Post", "Validate_Create")]
        public ValidationResponse ValidateCreate([FromBody]AdministrationUnitParameters value)
        {
            return _administrationUnitFacade.ValidateCreate(value);
        }

        [Routing("Post", "Create")]
        public ExecutionResponse Create([FromBody]AdministrationUnitParameters value)
        {
            _administrationUnitFacade.CreateNewAdministrationUnit(value);
            _postalCodeInfoFacade.CheckExistsPostalCode(value);
            return new ExecutionResponse(StatusCodes.Status201Created, null, new Error[0]);
        }

        [Routing("Put", "Validate_Edit")]
        public ValidationResponse ValidateEdit([FromBody]AdministrationUnitParameters value)
        {
            return _administrationUnitFacade.ValidateEdit(value);
        }

        [Routing("Put", "Edit")]
        public ExecutionResponse Edit([FromBody]AdministrationUnitParameters value)
        {
            _administrationUnitFacade.EditAdministrationUnit(value);
            _postalCodeInfoFacade.CheckExistsPostalCode(value);
            return new ExecutionResponse(StatusCodes.Status201Created, null, new Error[0]);
        }

        [Routing("Get", "List")]
        public ExecutionResponse List()
        {
            var result = Json.Serialize(_administrationUnitFacade.ListAdministrationUnits());
            return new ExecutionResponse(StatusCodes.Status200OK, result, new Error[0]);
        }

        [Routing("Get", "Load")]
        public ExecutionResponse Load(string id)
        {
            return _administrationUnitFacade.LoadAdministrationUnit(id);
        }
    }
}
