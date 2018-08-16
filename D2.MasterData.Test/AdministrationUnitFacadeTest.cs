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
    public class AdministrationUnitFacadeTest
    {
        [Fact(DisplayName = "Load with invalid id yields 422")]
        public void Load_with_invalid_id_yields_422()
        {
            var validator = Substitute.For<IParameterValidator>();
            var repository = Substitute.For<IAdministrationUnitsRepository>();
            var baseSettingsRepository = Substitute.For<IBaseSettingsRepository>();

            var facade = new AdministrationUnitFacade(repository, baseSettingsRepository, validator);
            var result = facade.LoadAdministrationUnit("");

            Assert.Equal(422, result.code);
        }

        [Fact(DisplayName = "Load with unknown id yields 404")]
        public void Load_with_unknown_id_yields_404()
        {
            var validator = Substitute.For<IParameterValidator>();
            var repository = Substitute.For<IAdministrationUnitsRepository>();
            var baseSettingsRepository = Substitute.For<IBaseSettingsRepository>();

            var facade = new AdministrationUnitFacade(repository, baseSettingsRepository, validator);
            var result = facade.LoadAdministrationUnit(Guid.NewGuid().ToString());

            Assert.Equal(404, result.code);
        }

        [Fact(DisplayName = "Load with known id yields 200 and valid json object")]
        public void Load_with_known_id_yields_200_and_valid_json_object()
        {
            var validator = Substitute.For<IParameterValidator>();
            var repository = Substitute.For<IAdministrationUnitsRepository>();
            var baseSettingsRepository = Substitute.For<IBaseSettingsRepository>();
            var unitId = Guid.NewGuid();
            
            repository.Load(Arg.Any<Guid>()).Returns(AdministrationUnitBuilder.New.WithId(unitId).Build());

            var facade = new AdministrationUnitFacade(repository, baseSettingsRepository, validator);
            var result = facade.LoadAdministrationUnit(unitId.ToString());

            Assert.Equal(200, result.code);
            Assert.False(string.IsNullOrWhiteSpace(result.json));
        }

        [Fact(DisplayName = "ValidateCreate with invalid object yields ExternalFailure")]
        public void ValidateCreate_with_invalid_object_yields_ExternalFailure()
        {
            var validator = Substitute.For<IParameterValidator>();
            validator
                .Validate(Arg.Any<AdministrationUnitParameters>(), RequestType.Post)
                .Returns(info => {
                    var validation = new ValidationResult();
                    validation.AddError("test", "Fehler");
                    return validation;
                });
            var repository = Substitute.For<IAdministrationUnitsRepository>();
            var baseSettingsRepository = Substitute.For<IBaseSettingsRepository>();

            var facade = new AdministrationUnitFacade(repository, baseSettingsRepository, validator);
            var result = facade.ValidateCreate(new AdministrationUnitParameters());
            Assert.Equal(State.ExternalFailure, result.result);
        }

        [Fact(DisplayName = "ValidateCreate with valid object yields NoError")]
        public void ValidateCreate_with_valid_object_yields_NoError()
        {
            var validator = Substitute.For<IParameterValidator>();
            validator
                .Validate(Arg.Any<AdministrationUnitParameters>(), RequestType.Post)
                .Returns(new ValidationResult());
            var repository = Substitute.For<IAdministrationUnitsRepository>();
            var baseSettingsRepository = Substitute.For<IBaseSettingsRepository>();

            var facade = new AdministrationUnitFacade(repository, baseSettingsRepository, validator);

            var result = facade.ValidateCreate(new AdministrationUnitParameters());
            Assert.Equal(State.NoError, result.result);
        }

        [Fact(DisplayName = "Create AdministrationUnitProperty from BaseSettings is valid")]
        public void Create_AdministrationUnitProperty_from_BaseSettings_is_valid()
        {
            var validator = Substitute.For<IParameterValidator>();
            var repository = Substitute.For<IAdministrationUnitsRepository>();
            var baseSettingsRepository = Substitute.For<IBaseSettingsRepository>();
            var hasProperties = false;

            repository.Insert(Arg.Do<AdministrationUnit>(unit => hasProperties = unit.AdministrationUnitProperties.Any()));

            var facade = new AdministrationUnitFacade(repository, baseSettingsRepository, validator);
            AdministrationUnitsFeature unitsFeature = AdministrationUnitsFeatureBuilder.New.WithId(Guid.NewGuid()).Build();
            baseSettingsRepository.List().Returns(new[] { unitsFeature });
            AdministrationUnitParameters adminUnitParameters = AdministrationUnitParametersBuilder.New.Build();
            facade.CreateNewAdministrationUnit(adminUnitParameters);

            Assert.True(hasProperties);
        }

        [Fact(DisplayName = "AdministrationUnitsFeature AddProperty can add properties to two AdminUnits")]
        public void AdministrationUnitsFeature_AddProperty_can_add_properties_to_two_AdminUnits()
        {
            var validator = Substitute.For<IParameterValidator>();
            var repository = Substitute.For<IAdministrationUnitsRepository>();
            var baseSettingsRepository = Substitute.For<IBaseSettingsRepository>();

            var facade = new AdministrationUnitFacade(repository, baseSettingsRepository, validator);
            AdministrationUnit adminUnit1 = AdministrationUnitBuilder.New.WithId(Guid.NewGuid()).Build();
            AdministrationUnit adminUnit2 = AdministrationUnitBuilder.New.WithId(Guid.NewGuid()).Build();
            Guid [] adminisstrationUnitIds = { adminUnit1.Id, adminUnit2.Id };

            repository.Load(adminUnit1.Id).Returns(adminUnit1);
            repository.Load(adminUnit2.Id).Returns(adminUnit2);
            var parameters = new SelectedAdministrationUnitPropertyParameters();
            parameters.AdministrationUnitIds = adminisstrationUnitIds;

            AdministrationUnitsFeatureParameters featureParameters = AdministrationUnitsFeatureParametersBuilder.New.Build();
            parameters.AdministrationUnitsFeatureParameters = featureParameters;
            facade.AddAdministrationUnitProperty(parameters);

            Assert.True(adminUnit1.AdministrationUnitProperties.Any());
            Assert.True(adminUnit2.AdministrationUnitProperties.Any());

        }
    }
}
