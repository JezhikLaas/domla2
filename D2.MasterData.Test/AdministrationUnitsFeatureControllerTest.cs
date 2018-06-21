using NSubstitute;
using D2.MasterData.Facades;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using D2.MasterData.Controllers;

namespace D2.MasterData.Test
{
    public class AdministrationUnitsFeatureControllerTest
    {
        [Fact(DisplayName = "AdministrationUnitsFeatureController.Post calls CreateNewAdministrationUnitsFeature")]
        public void AdministrationUnitsController_Post_calls_CreateNewAdministrationUnitFeature()
        {
            var baseSettingsFacade = Substitute.For<IBaseSettingsFacade>();
            var postalCodeFacade = Substitute.For<IPostalCodeInfoFacade>();
            var controller = new BaseSettingsController(baseSettingsFacade);

            controller.Create(null);
            baseSettingsFacade.Received(1).CreateNewAdministrationUnitsFeature(null);
        }
    }
}
