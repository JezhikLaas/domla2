using D2.MasterData.Controllers;
using D2.MasterData.Facades;
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
            var controller = new AdministrationUnitController(facade);

            controller.Create(null);
            facade.Received(1).CreateNewAdministrationUnit(null);
        }
    }
}
