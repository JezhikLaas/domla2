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
            var adminUnitFacade = Substitute.For<IAdministrationUnitFacade>();
            var postalCodeFacade = Substitute.For<IPostalCodeInfoFacade>();
            var controller = new AdministrationUnitController(adminUnitFacade, postalCodeFacade);

            controller.Create(null);
            adminUnitFacade.Received(1).CreateNewAdministrationUnit(null);
        }

        [Fact(DisplayName = "AdministrationUnitController.Post calls CheckExistsPostalCode")]
        public void AdministrationUnitController_Post_calls_CheckExistsPostalCode()
        {
            var adminUnitFacade = Substitute.For<IAdministrationUnitFacade>();
            var postalCodeFacade = Substitute.For<IPostalCodeInfoFacade>();
            var controller = new AdministrationUnitController(adminUnitFacade, postalCodeFacade);

            controller.Create(null);
            postalCodeFacade.Received(1).CheckExistsPostalCode(null);
        }
    }
}
