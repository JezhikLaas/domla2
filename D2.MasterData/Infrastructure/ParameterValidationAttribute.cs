using System;
using System.Collections.Generic;

namespace D2.MasterData.Infrastructure
{
    public abstract class ParameterValidationAttribute : Attribute
    {
        List<RequestType> _requestTypes;

        protected ParameterValidationAttribute(params RequestType[] requestTypes)
        {
            _requestTypes = new List<RequestType>(requestTypes);
        }

        public IReadOnlyList<RequestType> RequestTypes
        {
            get { return _requestTypes; }
        }

        public abstract string Error(IParameterValidator validator, object value, Type propertyType);
    }
}
