using D2.MasterData.Facades;
using D2.MasterData.Parameters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace D2.MasterData.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("[controller]")]
    public class AdministrationUnitController : Controller
    /* Instanz dieser Klasse und alle Insatnzen darunterliegenden Klassen werden von Dependency Injection
       automatisch  erzeugt */
    {
        IAdministrationUnitFacade _administrationUnitFacade;

        // Konstruktor
        public AdministrationUnitController(
            IAdministrationUnitFacade administrationUnitFacade)
        {
            _administrationUnitFacade = administrationUnitFacade;
        }

        [HttpPost]
        public IActionResult Post([FromBody]AdministrationUnitPostParameters value)
        {
            var result = _administrationUnitFacade.CreateNewAdministrationUnit(value);

            if (result.IsValid) return StatusCode(StatusCodes.Status201Created);

            return StatusCode(StatusCodes.Status422UnprocessableEntity);
        }
    }
}
