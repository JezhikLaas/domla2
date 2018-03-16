module D2
{
    module Service
    {
        module Contracts
        {
            module Common
            {
                struct Error
                {
                    string property;
                    string description;
                };

				struct Parameter
				{
					string name;
					string value;
				};

                sequence<Error> Errors;
                sequence<Parameter> Parameters;
            }
            
            module Validation
            {
				enum State { NoError = 0, ExternalFailure, InternalFailure }

                struct ValidationResponse
                {
                    State result;
                    Common::Errors errors;
                };

                struct ValidationRequest
                {
                    string topic;
                    string action;
                    string json;
					Common::Parameters parameters;
                };

                interface Validator
                {
                    ValidationResponse validate(ValidationRequest request);
                };
            }
        }
    }
}