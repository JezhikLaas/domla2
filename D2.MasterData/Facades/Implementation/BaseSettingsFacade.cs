using D2.Infrastructure;
using D2.MasterData.Infrastructure;
using D2.MasterData.Models;
using D2.MasterData.Parameters;
using D2.MasterData.Repositories;
using D2.Service.Contracts.Common;
using D2.Service.Contracts.Execution;
using D2.Service.Contracts.Validation;
using D2.Service.IoC;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace D2.MasterData.Facades.Implementation
{
    [RequestScope]
    public class BaseSettingsFacade : IBaseSettingsFacade
    {
        readonly IBasicSettingsRepository _repository;
        readonly IParameterValidator _parameterValidator;
        readonly IAdministrationUnitPropertyRepository _administrationUnitPropertyRepository;
        readonly IAdministrationUnitsRepository _administrationUnitsRepository;

        public BaseSettingsFacade (
            IBasicSettingsRepository repository,
            IAdministrationUnitsRepository administrationUnitsRepository,
            IAdministrationUnitPropertyRepository administrationUnitPropertyRepository,
            IParameterValidator parameterValidator)
        {
            _repository = repository;
            _parameterValidator = parameterValidator;
            _administrationUnitsRepository = administrationUnitsRepository;
            _administrationUnitPropertyRepository = administrationUnitPropertyRepository;
        }

        public void CreateNewAdministrationUnitsFeature(AdministrationUnitsFeatureParameters value)
        {
            var AdministrationUnitFeature = new AdministrationUnitsFeature(value);
            _repository.Insert(AdministrationUnitFeature);
        }

        public void EditAdministrationUnitsFeature(AdministrationUnitsFeatureParameters value)
        {
            var AdministrationUnitFeature = new AdministrationUnitsFeature(value);
            _repository.Update(AdministrationUnitFeature);
        }

        public IEnumerable<AdministrationUnitsFeature> ListAdministrationUnitsFeatures()
        {
            return _repository.List();
        }

        public ExecutionResponse LoadAdministrationUnitsFeature(string id)
        {
            Guid unitId;
            if (Guid.TryParse(id, out unitId) == false)
            {
                return new ExecutionResponse(
                    StatusCodes.Status422UnprocessableEntity,
                    null,
                    new[] { new Error("id", "must be a Guid") }
                );
            }

            var unit = _repository.Load(unitId);
            if (unit == null || unit.Id != unitId)
            {
                return new ExecutionResponse(StatusCodes.Status404NotFound, null, new Error[0]);
            }

            return new ExecutionResponse(StatusCodes.Status200OK, Json.Serialize(unit), new Error[0]);
        }

        public ValidationResponse ValidateCreate(AdministrationUnitsFeatureParameters value)
        {
            var result = _parameterValidator.Validate(value, RequestType.Post);

            if (result.IsValid)
            {
                return new ValidationResponse(State.NoError, new Error[0]);
            }

            var errors = result
                            .Errors
                            .Select(error => new Error(error.Property, error.Error))
                            .ToArray();

            return new ValidationResponse(State.ExternalFailure, errors);
        }

        public ValidationResponse ValidateEdit(AdministrationUnitsFeatureParameters value)
        {
            var result = _parameterValidator.Validate(value, RequestType.Put);

            if (result.IsValid)
            {
                return new ValidationResponse(State.NoError, new Error[0]);
            }

            var errors = result
                            .Errors
                            .Select(error => new Error(error.Property, error.Error))
                            .ToArray();

            return new ValidationResponse(State.ExternalFailure, errors);
        }


        public void CreateNewAdministratioUnitPropertyForAllAdministraionUnits(AdministrationUnitsFeatureParameters value)
        {
            var result = from unit in _administrationUnitsRepository.List()
                         select unit;
            foreach(AdministrationUnit administrationUnit in result)
            {
                if (administrationUnit.Id != value.InitialAdministrationUnitId) continue;
                AdministrationUnitPropertyParameters parameter = new AdministrationUnitPropertyParameters();
                parameter.Title = value.Title;
                parameter.Description = value.Description;
                switch (value.Tag)
                {
                    case VariantTag.DateTime:
                        parameter.Value = new Variant(new DateTime());
                        break;
                    case VariantTag.String:
                        parameter.Value = new Variant(string.Empty);
                        break;
                    case VariantTag.TypedValue:
                        parameter.Value = new Variant(new TypedValue(0, value.TypedValueUnit, value.TypedValueDecimalPlace));
                        break;
                    default:
                        break;
                }

                var administrationUnitProperty = new AdministrationUnitProperty(parameter, administrationUnit);
                administrationUnit.AddProperty(administrationUnitProperty);
            }
        }
    }
}
