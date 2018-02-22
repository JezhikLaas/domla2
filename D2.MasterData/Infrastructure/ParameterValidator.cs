using System.Linq;

namespace D2.MasterData.Infrastructure
{
    public class ParameterValidator : IParameterValidator
    {
        public ValidationResult Validate(object requestParameters, RequestType requestType)
        {
            var Result = new ValidationResult();

            foreach (var property in requestParameters.GetType().GetProperties())
            {
                var attributes = property.GetCustomAttributes(typeof(ParameterValidationAttribute), true).OfType<ParameterValidationAttribute>();
                foreach (var attribute in attributes)
                {
                    if (attribute.RequestTypes.Any() == false || attribute.RequestTypes.Contains(requestType))
                    {
                        var ValidationFailure = attribute.Error(this, property.GetValue(this), property.PropertyType);
                        if (ValidationFailure != null)
                        {
                            Result.AddError(property.Name, ValidationFailure);
                        }
                    }
                }
            }

            return Result;
        }
    }
}
