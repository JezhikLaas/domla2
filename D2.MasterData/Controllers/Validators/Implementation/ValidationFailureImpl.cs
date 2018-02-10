namespace D2.MasterData.Controllers.Validators.Implementation
{
    public class ValidationFailureImpl : ValidationFailure
    {
        private readonly string _propertyName;
        private readonly string _errorMessage;

        internal ValidationFailureImpl(string propertyName, string errorMessage)
        {
            _propertyName = propertyName;
            _errorMessage = errorMessage;
        }

        public string PropertyName => _propertyName;

        public string ErrorMessage => _errorMessage;
    }
}
