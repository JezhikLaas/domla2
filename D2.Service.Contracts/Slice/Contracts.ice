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
                
				struct Request
                {
                    string topic;
                    string action;
                    string json;
					Common::Parameters parameters;
                };
            }
            
            module Validation
            {
				enum State { NoError = 0, ExternalFailure, InternalFailure }

                struct ValidationResponse
                {
                    State result;
                    Common::Errors errors;
                };

                interface Validator
                {
                    ValidationResponse validate(Common::Request request);
                };
            }

			module Execution
			{
				struct ExecutionResponse
                {
                    int code;
					string json;
                    Common::Errors errors;
                };

				interface Executor
				{
					ExecutionResponse execute(Common::Request request);
				};
			}
        }
    }
}