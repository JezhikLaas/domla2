using D2.MasterData.Facades;
using D2.MasterData.Infrastructure;
using D2.MasterData.Parameters;
using D2.Service.Contracts.Common;
using D2.Service.Contracts.Execution;
using D2.Service.Contracts.Validation;
using D2.Service.Controller;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace D2.MasterData.Controllers
{
    /// <summary>
    /// Instanz dieser Klasse und alle Instanzen darunterliegender Klassen werden
    /// von Dependency Injection automatisch erzeugt
    /// </summary>
    public class AdministrationUnitController : BaseController
    {
        IAdministrationUnitFacade _administrationUnitFacade;
        IParameterValidator _parameterValidator;

        /// <summary>
        /// Konstruktor.
        /// </summary>
        /// <param name="administrationUnitFacade">Fassade für fachliche Aufgaben.</param>
        /// <param name="parameterValidator">Validator für Parameter, die an Endpunkte gesendet werden.</param>
        public AdministrationUnitController(
            IAdministrationUnitFacade administrationUnitFacade,
            IParameterValidator parameterValidator)
        {
            _administrationUnitFacade = administrationUnitFacade;
            _parameterValidator = parameterValidator;
        }

        [Routing("Post", "Validate_Create")]
        public ValidationResponse ValidateCreate([FromBody]AdministrationUnitParameters value)
        {
            var result = _parameterValidator.Validate(value, RequestType.Post);

            if (result.IsValid)
            {
                return new ValidationResponse(State.NoError, new Error[0]);
            }

            var errors = result
                            .Errors
                            .Select(error => new Error(error.Property, error.Error))
                            .ToArray();

            return new ValidationResponse(State.ExternalFailure, errors);
        }

        [Routing("Post", "Create")]
        public ExecutionResponse Create([FromBody]AdministrationUnitParameters value)
        {
            _administrationUnitFacade.CreateNewAdministrationUnit(value);
            return new ExecutionResponse(StatusCodes.Status201Created, null, new Error[0]);
        }

        [Routing("Get", "List")]
        public ExecutionResponse List()
        {
            var result = Json.Serialize(_administrationUnitFacade.ListAdministrationUnits());
            return new ExecutionResponse(StatusCodes.Status200OK, result, new Error[0]);
        }
    }
}
