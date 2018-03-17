using D2.MasterData.Infrastructure.Validation;
using D2.Service.IoC;
using System.Collections.Generic;
using System.Linq;

namespace D2.MasterData.Infrastructure
{
    [Singleton]
    public class ParameterValidator : IParameterValidator
    {
        public ValidationResult Validate(object requestParameters, RequestType requestType)
        {
            var Result = new ValidationResult();
            var processed = new HashSet<object>();

            InternalValidate(requestParameters, requestType, Result, processed);

            return Result;
        }

        private void InternalValidate(object requestParameters, RequestType requestType, ValidationResult result, HashSet<object> processed)
        {
            if (requestParameters == null || processed.Contains(requestParameters)) return;
            processed.Add(requestParameters);

            foreach (var property in requestParameters.GetType().GetProperties()) {
                var attributes = property.GetCustomAttributes(typeof(ParameterValidationAttribute), true).OfType<ParameterValidationAttribute>();
                foreach (var attribute in attributes) {
                    if (attribute.RequestTypes.Any() == false || attribute.RequestTypes.Contains(requestType)) {
                        var value = property.GetValue(requestParameters);
                        var collection = value as IEnumerable<object>;

                        if (collection != null) {
                            foreach (var item in collection) {
                                InternalValidate(item, requestType, result, processed);
                            }
                        }
                        else {
                            InternalValidate(value, requestType, result, processed);
                        }

                        var ValidationFailure = attribute.Error(this, value, property.PropertyType);
                        if (ValidationFailure != null) {
                            result.AddError(property.Name, ValidationFailure);
                        }
                    }
                }
            }
        }
    }
}
