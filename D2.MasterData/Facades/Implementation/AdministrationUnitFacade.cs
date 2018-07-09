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
    public class AdministrationUnitFacade : IAdministrationUnitFacade
    {
        private readonly IAdministrationUnitsRepository _repository;
        private readonly IBaseSettingsRepository _basicSettingsRepository;
        private readonly IParameterValidator _parameterValidator;

        public AdministrationUnitFacade(
            IAdministrationUnitsRepository repository,
            IBaseSettingsRepository basicSettingsRepository,
            IParameterValidator parameterValidator)
        {
            _repository = repository;
            _basicSettingsRepository = basicSettingsRepository;
            _parameterValidator = parameterValidator;
        }

        public Guid CreateNewAdministrationUnit(AdministrationUnitParameters value)
        {
            var administrationUnit = new AdministrationUnit(value);

            foreach (var feature in _basicSettingsRepository.List())
            {
                var propertyParameters = new AdministrationUnitPropertyParameters {
                    Description = feature.Description,
                    Title = feature.Title
                };
                
                switch (feature.Tag)
                {
                    case VariantTag.DateTime:
                        propertyParameters.Value = new Variant(new DateTime());
                        break;
                    case VariantTag.String:
                        propertyParameters.Value = new Variant(string.Empty);
                        break;
                    case VariantTag.TypedValue:
                        propertyParameters.Value = new Variant(new TypedValue(0, feature.TypedValueUnit, feature.TypedValueDecimalPlace));
                        break;
                    case VariantTag.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                var admnistrationUnitProperty = new AdministrationUnitProperty(propertyParameters, administrationUnit);
                administrationUnit.AddProperty(admnistrationUnitProperty);
            }
            _repository.Insert(administrationUnit);
            return administrationUnit.Id;
        }

        public void EditAdministrationUnit(AdministrationUnitParameters value)
        {
            var administrationUnit = new AdministrationUnit(value);
            _repository.Update(administrationUnit);
        }

        public IEnumerable<AdministrationUnit> ListAdministrationUnits()
        {
            return _repository.List();
        }

        public ExecutionResponse LoadAdministrationUnit(string id)
        {
            if (Guid.TryParse(id, out var unitId) == false) {
                return new ExecutionResponse(
                    StatusCodes.Status422UnprocessableEntity,
                    null,
                    new[] { new Error("id", "must be a Guid") }
                );
            }

            var unit = _repository.Load(unitId);
            if (unit == null || unit.Id != unitId) {
                return new ExecutionResponse(StatusCodes.Status404NotFound, null, new Error[0]);
            }

            return new ExecutionResponse(StatusCodes.Status200OK, Json.Serialize(unit), new Error[0]);
        }

        public ValidationResponse ValidateCreate(AdministrationUnitParameters value)
        {
            var result = _parameterValidator.Validate(value, RequestType.Post);

            if (result.IsValid) {
                return new ValidationResponse(State.NoError, new Error[0]);
            }

            var errors = result
                            .Errors
                            .Select(error => new Error(error.Property, error.Error))
                            .ToArray();

            return new ValidationResponse(State.ExternalFailure, errors);
        }

        public ValidationResponse ValidateEdit(AdministrationUnitParameters value)
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
    }
}
