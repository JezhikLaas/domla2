﻿using D2.Infrastructure;
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
        readonly IBaseSettingsRepository _repository;
        readonly IParameterValidator _parameterValidator;
        readonly IAdministrationUnitsRepository _administrationUnitsRepository;

        public BaseSettingsFacade (
            IBaseSettingsRepository repository,
            IAdministrationUnitsRepository administrationUnitsRepository,
            IParameterValidator parameterValidator)
        {
            _repository = repository;
            _parameterValidator = parameterValidator;
            _administrationUnitsRepository = administrationUnitsRepository;
        }

        public void CreateNewAdministrationUnitsFeature(AdministrationUnitsFeatureParameters value)
        {
            var administrationUnitFeature = new AdministrationUnitsFeature(value);
            _repository.Insert(administrationUnitFeature);
        }

        public void EditAdministrationUnitsFeature(AdministrationUnitsFeatureParameters value)
        {
            var administrationUnitFeature = new AdministrationUnitsFeature(value);
            _repository.Update(administrationUnitFeature);
        }

        public IEnumerable<AdministrationUnitsFeature> ListAdministrationUnitsFeatures()
        {
            return _repository.List();
        }

        public ExecutionResponse LoadAdministrationUnitsFeature(string id)
        {
            if (Guid.TryParse(id, out var unitId) == false)
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
            foreach(var administrationUnit in _administrationUnitsRepository.List())
            {
                var parameter = new AdministrationUnitPropertyParameters {
                    Title = value.Title,
                    Description = value.Description
                };
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
                    case VariantTag.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                var administrationUnitProperty = new AdministrationUnitProperty(parameter, administrationUnit);
                administrationUnit.AddProperty(administrationUnitProperty);
                _administrationUnitsRepository.Modify(administrationUnit);
            }
        }
    }
}
