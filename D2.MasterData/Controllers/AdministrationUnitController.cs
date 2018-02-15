using D2.MasterData.Facades;
using D2.MasterData.Infrastructure;
using D2.MasterData.Parameters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace D2.MasterData.Controllers
{
    /// <summary>
    /// Instanz dieser Klasse und alle Instanzen darunterliegender Klassen werden
    /// von Dependency Injection automatisch erzeugt
    /// </summary>
    [Route("[controller]")]
    public class AdministrationUnitController : Controller
    {
        IAdministrationUnitFacade _administrationUnitFacade;
        IParameterValidator _parameterValidator;

        /// <summary>
        /// Konstruktor.
        /// </summary>
        /// <param name="administrationUnitFacade">Fassade für fachliche Aufgaben.</param>
        public AdministrationUnitController(
            IAdministrationUnitFacade administrationUnitFacade,
            IParameterValidator parameterValidator)
        {
            _administrationUnitFacade = administrationUnitFacade;
            _parameterValidator = parameterValidator;
        }

        [HttpPost("create")]
        public IActionResult Post([FromBody]AdministrationUnitParameters value)
        {
            var result = _parameterValidator.Validate(value, RequestType.Post);

            if (result.IsValid)
            {
                _administrationUnitFacade.CreateNewAdministrationUnit(value);
                return StatusCode(StatusCodes.Status201Created);
            }

            return StatusCode(StatusCodes.Status422UnprocessableEntity);
        }
    }
}
