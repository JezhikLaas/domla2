using System;
using System.Collections;

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

            if (value is IEnumerable list) {
                var count = 0;
                foreach (var _ in list) {
                    ++count;
                }
                if (count > _maxLength) return "container contains too much elements";
            }
            
            return null;
        }
    }
}