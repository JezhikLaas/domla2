using System.Collections.Generic;
using System.Linq;

namespace D2.MasterData.Infrastructure
{
    public class ValidationResult
    {
        private List<ValidationError> _errors;

        public ValidationResult()
        {
            _errors = new List<ValidationError>();
        }

        public bool IsValid
        {
            get { return _errors.Any() == false; }
        }

        public IEnumerable<ValidationError> Errors
        {
            get { return _errors; }
        }

        internal void AddError(string name, string error)
        {
            _errors.Add(new ValidationError(name, error));
        }
    }
}

