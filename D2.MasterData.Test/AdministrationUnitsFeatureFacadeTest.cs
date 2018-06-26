using D2.MasterData.Facades.Implementation;
using D2.MasterData.Infrastructure;
using D2.MasterData.Models;
using D2.MasterData.Parameters;
using D2.MasterData.Repositories;
using D2.MasterData.Test.Helper;
using D2.Service.Contracts.Validation;
using NSubstitute;
using System;
using System.Linq;
using Xunit;

namespace D2.MasterData.Test
{
    public class AdministrationUnitsFeatureFacadeTest
    {
        [Fact(DisplayName = "AdministrationUnitsFeature load with invalid id yields 422")]
        public void AdministrationUnitsFeature_load_with_invalid_id_yields_422()
        {
            var validator = Substitute.For<IParameterValidator>();
            var repository = Substitute.For<IBaseSettingsRepository>();
            var administrationUnitRepository = Substitute.For<IAdministrationUnitsRepository>();

            var facade = new BaseSettingsFacade(repository, administrationUnitRepository, validator);
            var result = facade.LoadAdministrationUnitsFeature("");

            Assert.Equal(422, result.code);
        }

        [Fact(DisplayName = "AdministrationUnitsFeature load with unknown id yields 404")]
        public void AdministrationUnitsFeature_load_with_unknown_id_yields_404()
        {
            var validator = Substitute.For<IParameterValidator>();
            var repository = Substitute.For<IBaseSettingsRepository>();
            var administrationUnitRepository = Substitute.For<IAdministrationUnitsRepository>();

            var facade = new BaseSettingsFacade(repository, administrationUnitRepository, validator);
            var result = facade.LoadAdministrationUnitsFeature(Guid.NewGuid().ToString());

            Assert.Equal(404, result.code);
        }

        [Fact(DisplayName = "AdministrationUnitsFeature load with known id yields 200 and valid json object")]
        public void AdministrationUnitsFeature_load_with_known_id_yields_200_and_valid_json_object()
        {
            var validator = Substitute.For<IParameterValidator>();
            var repository = Substitute.For<IBaseSettingsRepository>();
            var administrationUnitRepository = Substitute.For<IAdministrationUnitsRepository>();
            var unitId = Guid.NewGuid();

            repository.Load(Arg.Any<Guid>()).Returns(AdministrationUnitsFeatureBuilder.New.WithId(unitId).Build());

            var facade = new BaseSettingsFacade(repository, administrationUnitRepository, validator);
            var result = facade.LoadAdministrationUnitsFeature(unitId.ToString());

            Assert.Equal(200, result.code);
            Assert.False(string.IsNullOrWhiteSpace(result.json));
        }

        [Fact(DisplayName = "AdministrationUnitsFeature validateCreate with invalid object yields ExternalFailure")]
        public void AdministrationUnitsFeature_validateCreate_with_invalid_object_yields_ExternalFailure()
        {
            var validator = Substitute.For<IParameterValidator>();
            validator
                .Validate(Arg.Any<AdministrationUnitsFeatureParameters>(), RequestType.Post)
                .Returns(info => {
                    var validation = new ValidationResult();
                    validation.AddError("test", "Fehler");
                    return validation;
                });
            var repository = Substitute.For<IBaseSettingsRepository>();
            var administrationUnitRepository = Substitute.For<IAdministrationUnitsRepository>();
            var facade = new BaseSettingsFacade(repository, administrationUnitRepository, validator);

            var result = facade.ValidateCreate(new AdministrationUnitsFeatureParameters());
            Assert.Equal(State.ExternalFailure, result.result);
        }

        [Fact(DisplayName = "AdministrationUnitsFeature_validateCreate with valid object yields NoError")]
        public void AdministrationUnitsFeature_validateCreate_with_valid_object_yields_NoError()
        {
            var validator = Substitute.For<IParameterValidator>();
            validator
                .Validate(Arg.Any<AdministrationUnitsFeatureParameters>(), RequestType.Post)
                .Returns(new ValidationResult());
            var repository = Substitute.For<IBaseSettingsRepository>();
            var administrationUnitRepository = Substitute.For<IAdministrationUnitsRepository>();
            var facade = new BaseSettingsFacade(repository, administrationUnitRepository, validator);

            var result = facade.ValidateCreate(new AdministrationUnitsFeatureParameters());
            Assert.Equal(State.NoError, result.result);
        }

        [Fact(DisplayName = "AdministrationUnitsFeature_CreateNewAdminisstrationUnitPropertyForAllAdministrationUnits with valid object yields NoError")]
        public void AdministrationUnitsFeature_CreateNewAdminisstrationUnitPropertyForAllAdministrationUnits_with_valid_object_yields_NoError()
        {
            var validator = Substitute.For<IParameterValidator>();
            var repository = Substitute.For<IBaseSettingsRepository>();
            var administrationUnitRepository = Substitute.For<IAdministrationUnitsRepository>();


            var facade = new BaseSettingsFacade(repository, administrationUnitRepository, validator);
            AdministrationUnit adminUnit = AdministrationUnitBuilder.New.WithId(Guid.NewGuid()).Build();
            administrationUnitRepository.List().Returns(new[] { adminUnit });
            AdministrationUnitsFeatureParameters featureParameters = AdministrationUnitsFeatureParametersBuilder.New.Build();
            facade.CreateNewAdministratioUnitPropertyForAllAdministraionUnits(featureParameters);

            Assert.True(adminUnit.AdministrationUnitProperties.Any());
        }

        [Fact(DisplayName = "AdministrationUnitsFeature_CreateNewAdminisstrationUnitPropertyForAllAdministrationUnits_two AdminUnits")]
        public void AdministrationUnitsFeature_CreateNewAdminisstrationUnitPropertyForAllAdministrationUnits_two_AdminUnits()
        {
            var validator = Substitute.For<IParameterValidator>();
            var repository = Substitute.For<IBaseSettingsRepository>();
            var administrationUnitRepository = Substitute.For<IAdministrationUnitsRepository>();


            var facade = new BaseSettingsFacade(repository, administrationUnitRepository, validator);
            AdministrationUnit adminUnit1 = AdministrationUnitBuilder.New.WithId(Guid.NewGuid()).Build();
            AdministrationUnit adminUnit2 = AdministrationUnitBuilder.New.WithId(Guid.NewGuid()).Build();
            administrationUnitRepository.List().Returns(new[] { adminUnit1, adminUnit2 });
            AdministrationUnitsFeatureParameters featureParameters = AdministrationUnitsFeatureParametersBuilder.New.Build();
            facade.CreateNewAdministratioUnitPropertyForAllAdministraionUnits(featureParameters);

            Assert.True(adminUnit1.AdministrationUnitProperties.Any());
            Assert.True(adminUnit2.AdministrationUnitProperties.Any());

        }
    }
}
