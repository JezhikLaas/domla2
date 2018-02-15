using System;

namespace D2.MasterData.Infrastructure
{
    public class NotNullOrEmptyAttribute : ParameterValidationAttribute
    {
        public NotNullOrEmptyAttribute(params RequestType[] requestTypes)
            : base(requestTypes)
        { }

        public override string Error(IParameterValidator validator, object value, Type propertyType)
        {
            if (value == null)
            {
                return "must not be null";
            }

            if (propertyType == typeof(string) && string.IsNullOrEmpty(value as string))
            {
                return "must not be empty";
            }

            return null;
        }
    }
}
