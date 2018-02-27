using System;
using System.Collections;
using System.Linq;

namespace D2.MasterData.Infrastructure.Validation
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

            if (propertyType.GetInterfaces().Contains(typeof(IEnumerable)) && (((IEnumerable)value).GetEnumerator().MoveNext() == false)) {
                return "must not be empty";
            }

            return null;
        }
    }
}
