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
    public class PostalCodeInfoController : BaseController
    {
        IPostalCodeInfoFacade _postalCodeInfoFacade;

        /// <summary>
        /// Konstruktor.
        /// </summary>
        /// <param name="postalCodeInfoFacade">Fassade für fachliche Aufgaben.</param>
        public PostalCodeInfoController(
            IPostalCodeInfoFacade postalCodeInfoFacade)
        {
            _postalCodeInfoFacade = postalCodeInfoFacade;
        }

        [Routing("Post", "Validate_Create")]
        public ValidationResponse ValidateCreate([FromBody]PostalCodeInfoParameters value)
        {
            return _postalCodeInfoFacade.ValidateCreate(value);
        }

        [Routing("Post", "Create")]
        public ExecutionResponse Create([FromBody]PostalCodeInfoParameters value)
        {
            _postalCodeInfoFacade.CreateNewPostalCodeInfo(value);
            return new ExecutionResponse(StatusCodes.Status201Created, null, new Error[0]);
        }

        [Routing("Get", "List")]
        public ExecutionResponse List()
        {
            var result = Json.Serialize(_postalCodeInfoFacade.ListPostalCodeInfos());
            return new ExecutionResponse(StatusCodes.Status200OK, result, new Error[0]);
        }

        [Routing("Get", "Load")]
        public ExecutionResponse Load(string id)
        {
            return _postalCodeInfoFacade.LoadPostalCodeInfo(id);
        }
    }
}
