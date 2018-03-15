using D2.Service.Contracts.Validation;
using Ice;

namespace D2.Service.CallDispatcher
{
    public class Validator : ValidatorDisp_
    {
        public override ValidationResponse validate(ValidationRequest request, Current current = null)
        {
            throw new System.NotImplementedException();
        }
    }
}