using D2.MasterData.Facades.Implementation;
using D2.MasterData.Infrastructure;
using D2.MasterData.Parameters;
using D2.MasterData.Repositories;
using D2.MasterData.Test.Helper;
using D2.Service.Contracts.Validation;
using NSubstitute;
using System;
using Xunit;

namespace D2.MasterData.Test
{
    public class AdministrationUnitFeatureFacadeTest
    {
        [Fact(DisplayName = "AdministrationUnitFeature load with invalid id yields 422")]
        public void AdministrationUnitFeature_load_with_invalid_id_yields_422()
        {
            var validator = Substitute.For<IParameterValidator>();
            var repository = Substitute.For<IBasicSettingsRepository>();

            var facade = new BaseSettingsFacade(repository, validator);
            var result = facade.LoadAdministrationUnitFeature("");

            Assert.Equal(422, result.code);
        }

        [Fact(DisplayName = "AdministrationUnitFeature load with unknown id yields 404")]
        public void AdministrationUnitFeature_load_with_unknown_id_yields_404()
        {
            var validator = Substitute.For<IParameterValidator>();
            var repository = Substitute.For<IBasicSettingsRepository>();

            var facade = new BaseSettingsFacade(repository, validator);
            var result = facade.LoadAdministrationUnitFeature(Guid.NewGuid().ToString());

            Assert.Equal(404, result.code);
        }

        [Fact(DisplayName = "AdministrationUnitFeature load with known id yields 200 and valid json object")]
        public void AdministrationUnitFeature_load_with_known_id_yields_200_and_valid_json_object()
        {
            var validator = Substitute.For<IParameterValidator>();
            var repository = Substitute.For<IBasicSettingsRepository>();
            var unitId = Guid.NewGuid();

            repository.Load(Arg.Any<Guid>()).Returns(AdministrationUnitFeatureBuilder.New.WithId(unitId).Build());

            var facade = new BaseSettingsFacade(repository, validator);
            var result = facade.LoadAdministrationUnitFeature(unitId.ToString());

            Assert.Equal(200, result.code);
            Assert.False(string.IsNullOrWhiteSpace(result.json));
        }

        [Fact(DisplayName = "AdministrationUnitFeature validateCreate with invalid object yields ExternalFailure")]
        public void AdministrationUnitFeature_validateCreate_with_invalid_object_yields_ExternalFailure()
        {
            var validator = Substitute.For<IParameterValidator>();
            validator
                .Validate(Arg.Any<AdministrationUnitFeatureParameters>(), RequestType.Post)
                .Returns(info => {
                    var validation = new ValidationResult();
                    validation.AddError("test", "Fehler");
                    return validation;
                });
            var repository = Substitute.For<IBasicSettingsRepository>();
            var facade = new BaseSettingsFacade(repository, validator);

            var result = facade.ValidateCreate(new AdministrationUnitFeatureParameters());
            Assert.Equal(State.ExternalFailure, result.result);
        }

        [Fact(DisplayName = "AdministrationUnitFeature_validateCreate with valid object yields NoError")]
        public void AdministrationUnitFeature_validateCreate_with_valid_object_yields_NoError()
        {
            var validator = Substitute.For<IParameterValidator>();
            validator
                .Validate(Arg.Any<AdministrationUnitFeatureParameters>(), RequestType.Post)
                .Returns(new ValidationResult());
            var repository = Substitute.For<IBasicSettingsRepository>();
            var facade = new BaseSettingsFacade(repository, validator);

            var result = facade.ValidateCreate(new AdministrationUnitFeatureParameters());
            Assert.Equal(State.NoError, result.result);
        }
    }
}
