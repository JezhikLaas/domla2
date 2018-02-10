using System.Collections.Generic;
using System.Linq;

namespace D2.MasterData.Controllers.Validators.Implementation
{
    public class ValidationResultImpl : ValidationResult
    {
        private readonly List<ValidationFailure> _errors;

        internal ValidationResultImpl(FluentValidation.Results.ValidationResult result)
        {
            _errors = new List<ValidationFailure>();
            foreach (var error in result.Errors) {
                _errors.Add(new ValidationFailureImpl(error.PropertyName, error.ErrorMessage));
            }
        }

        public bool IsValid => _errors.Any() == false;

        public IEnumerable<ValidationFailure> Errors => _errors;
    }
}
