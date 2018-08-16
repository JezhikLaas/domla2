using D2.MasterData.Controllers;
using D2.MasterData.Facades;
using D2.MasterData.Models;
using D2.MasterData.Parameters;
using D2.MasterData.Test.Helper;
using NSubstitute;
using System;
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

        [Fact(DisplayName = "AdministrationUnitController.Put AddProperty calls AddProperty")]
        public void AdministrationUnitController_Put_AddProperty_calls_AddProperty()
        {
            var adminUnitFacade = Substitute.For<IAdministrationUnitFacade>();
            var postalCodeFacade = Substitute.For<IPostalCodeInfoFacade>();
            var controller = new AdministrationUnitController(adminUnitFacade, postalCodeFacade);
            AdministrationUnit adminUnit1 = AdministrationUnitBuilder.New.WithId(Guid.NewGuid()).Build();
            AdministrationUnit adminUnit2 = AdministrationUnitBuilder.New.WithId(Guid.NewGuid()).Build();
            Guid[] adminisstrationUnitIds = { adminUnit1.Id, adminUnit2.Id };
            var parameters = new SelectedAdministrationUnitPropertyParameters();
            controller.AddProperty(parameters);
            adminUnitFacade.Received(1).AddAdministrationUnitProperty(parameters);
            
        }
    }
}
