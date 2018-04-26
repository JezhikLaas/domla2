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
        readonly IAdministrationUnitRepository _repository;
        readonly IParameterValidator _parameterValidator;

        public AdministrationUnitFacade(
            IAdministrationUnitRepository repository,
            IParameterValidator parameterValidator)
        {
            _repository = repository;
            _parameterValidator = parameterValidator;
        }

        public void CreateNewAdministrationUnit(AdministrationUnitParameters value)
        {
            var administrationUnit = new AdministrationUnit(value);
            _repository.Insert(administrationUnit);
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
            Guid unitId;
            if (Guid.TryParse(id, out unitId) == false) {
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
