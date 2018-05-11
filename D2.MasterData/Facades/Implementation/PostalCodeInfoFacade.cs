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
    public class PostalCodeInfoFacade : IPostalCodeInfoFacade
    {
        readonly IPostalCodeInfoRepository _repository;
        readonly IParameterValidator _parameterValidator;

        public PostalCodeInfoFacade(
            IPostalCodeInfoRepository repository,
            IParameterValidator parameterValidator)
        {
            _repository = repository;
            _parameterValidator = parameterValidator;
        }

        public void CreateNewPostalCodeInfo(PostalCodeInfoParameters value)
        {
            var postalCodeInfo = new PostalCodeInfo(value);
            _repository.Insert(postalCodeInfo);
        }

        public IEnumerable<PostalCodeInfo> ListPostalCodeInfos()
        {
            return _repository.List();
        }

        public ExecutionResponse LoadPostalCodeInfo(string id)
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

        public ValidationResponse ValidateCreate(PostalCodeInfoParameters value)
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
   }
}
