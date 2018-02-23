using D2.MasterData.Controllers;
using D2.MasterData.Facades;
using D2.MasterData.Infrastructure;
using NSubstitute;
using Xunit;

namespace D2.MasterData.Test
{
    public class AdministrationUnitControllerTest
    {
        [Fact(DisplayName = "AdministrationUnitController.Post calls CreateNewAdministrationUnit")]
        public void AdministrationUnitController_Post_calls_CreateNewAdministrationUnit()
        {
            var facade = Substitute.For<IAdministrationUnitFacade>();
            var validator = Substitute.For<IParameterValidator>();
            validator.Validate(null, Arg.Any<RequestType>()).Returns(new ValidationResult());

            var controller = new AdministrationUnitController(facade, validator);

            controller.Post(null);
            facade.Received(1).CreateNewAdministrationUnit(null);
        }
    }
}
