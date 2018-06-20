using D2.MasterData.Facades;
using D2.MasterData.Facades.Implementation;
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
    public class BaseSettingsController : BaseController
    {
        IBaseSettingsFacade _BaseSettingsFacade;

        /// <summary>
        /// Konstruktor.
        /// </summary>
        /// <param name="AdministrationUnitFeatureFacade">Fassade für fachliche Aufgaben.</param>
        public BaseSettingsController(
            IBaseSettingsFacade basicSettingsFacade)
        {
            _BaseSettingsFacade = basicSettingsFacade;
        }

        [Routing("Post", "Validate_Create")]
        public ValidationResponse ValidateCreate([FromBody]AdministrationUnitFeatureParameters value)
        {
            return _BaseSettingsFacade.ValidateCreate(value);
        }

        [Routing("Post", "Create")]
        public ExecutionResponse Create([FromBody]AdministrationUnitFeatureParameters value)
        {
            _BaseSettingsFacade.CreateNewAdministrationUnitFeature(value);
            return new ExecutionResponse(StatusCodes.Status201Created, null, new Error[0]);
        }

        [Routing("Put", "Validate_Edit")]
        public ValidationResponse ValidateEdit([FromBody]AdministrationUnitFeatureParameters value)
        {
            return _BaseSettingsFacade.ValidateEdit(value);
        }

        [Routing("Put", "Edit")]
        public ExecutionResponse Edit([FromBody]AdministrationUnitFeatureParameters value)
        {
            _BaseSettingsFacade.EditAdministrationUnitFeature(value);
            return new ExecutionResponse(StatusCodes.Status201Created, null, new Error[0]);
        }

        [Routing("Get", "List")]
        public ExecutionResponse List()
        {
            var result = Json.Serialize(_BaseSettingsFacade.ListAdministrationUnitFeatures());
            return new ExecutionResponse(StatusCodes.Status200OK, result, new Error[0]);
        }

        [Routing("Get", "Load")]
        public ExecutionResponse Load(string id)
        {
            return _BaseSettingsFacade.LoadAdministrationUnitFeature(id);
        }
    }
}
