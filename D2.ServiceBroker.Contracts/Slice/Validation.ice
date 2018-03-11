module D2
{
    module ServiceBroker
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

                sequence<Error> Errors;
            }
            
            module Validation
            {
                struct ValidationResponse
                {
                    bool isOk;
                    Common::Errors errors;
                };

                struct ValidationRequest
                {
                    string verb;
                    string endpoint;
                    string json;
                };

                interface Validator
                {
                    ValidationResponse validate(ValidationRequest request);
                };
            }
        }
    }
}