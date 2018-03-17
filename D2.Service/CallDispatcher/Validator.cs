using D2.Service.Contracts.Common;
using D2.Service.Contracts.Validation;
using D2.Service.IoC;
using Ice;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;

namespace D2.Service.CallDispatcher
{
    public class Validator : ValidatorDisp_
    {
        ILogger<Validator> _logger;
        Dispatcher _dispatcher;

        public Validator(ILogger<Validator> logger, Dispatcher dispatcher)
        {
            _logger = logger;
            _dispatcher = dispatcher;
        }

        public override ValidationResponse validate(Contracts.Common.Request request, Current current = null)
        {
            var clock = new Stopwatch();
            ValidationResponse result = null;

            clock.Start();
            using (Scope.BeginScope()) {
                result = InternalValidate(request, current);
            }
            clock.Stop();

            _logger.LogDebug($"validated {request.topic}::{request.action} in {clock.ElapsedMilliseconds} msec");
            return result;
        }

        ValidationResponse InternalValidate(Contracts.Common.Request request, Current current)
        {
            _logger.LogDebug($"start validation request for {request.topic}::{request.action}");
            _logger.LogTrace($"with body {request.json ?? "<NONE>"} and {request.parameters}");
            try {
                var result = _dispatcher.PreCallCheck(
                    request.topic,
                    request.action,
                    request.json,
                    request.parameters.Select(parameter => new QueryParameter {
                        Name = parameter.name,
                        Value = parameter.value
                    }).ToList());

                return (ValidationResponse)result;
            }
            catch (Exception error) {
                _logger.LogError(error, $"validation of {request.topic}::{request.action} failed");
                return new ValidationResponse {
                    result = State.InternalFailure,
                    errors = new[] { new Error { property = "Exception", description = error.Message } }
                };
            }
        }
    }
}