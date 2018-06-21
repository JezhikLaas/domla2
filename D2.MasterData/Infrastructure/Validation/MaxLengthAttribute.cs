using System;
using System.Collections;
using System.Linq;

namespace D2.MasterData.Infrastructure.Validation
{
    public class MaxLengthAttribute : ParameterValidationAttribute
    {
        private readonly int _maxLength;

        public MaxLengthAttribute(int maxLength, params RequestType[] requestTypes)
            : base(requestTypes)
        {
            _maxLength = maxLength;
        }

        public override string Error(IParameterValidator validator, object value, Type propertyType)
        {
            var text = value as string;
            if (text != null && text.Length > _maxLength) return "text value contains too much characters";

            if (!(value is IEnumerable list)) return null;
            
            var count = list.Cast<object>().Count();
            if (count > _maxLength) return "container contains too much elements";

            return null;
        }
    }
}