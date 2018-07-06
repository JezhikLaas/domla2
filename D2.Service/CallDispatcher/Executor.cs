using D2.Service.Contracts.Common;
using D2.Service.Contracts.Execution;
using D2.Service.IoC;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using D2.Service.Controller;

namespace D2.Service.CallDispatcher
{
    public class Executor : ExecutorDisp_
    {
        private readonly ILogger<Executor> _logger;
        private readonly Dispatcher _dispatcher;
        private readonly IServices _resolver;

        public Executor(ILogger<Executor> logger, Dispatcher dispatcher, IServices resolver)
        {
            _logger = logger;
            _dispatcher = dispatcher;
            _resolver = resolver;
        }

        public override ExecutionResponse execute(Contracts.Common.Request request, Ice.Current current = null)
        {
            var clock = new Stopwatch();
            ExecutionResponse result = null;
            var context = current?.ctx ?? new Dictionary<string, string>();

            clock.Start();
            using (Scope.BeginScope()) {
                var callContext = _resolver.Resolve<ICallContext>();
                callContext.SetupContext(context);
                result = InternalExecute(request, current);
            }
            clock.Stop();

            _logger.LogDebug($"executed {request.topic}::{request.action} in {clock.ElapsedMilliseconds} msec");
            return result;
        }

        ExecutionResponse InternalExecute(Contracts.Common.Request request, Ice.Current current)
        {
            _logger.LogDebug($"start execution request for {request.topic}::{request.action}");
            _logger.LogTrace($"with body {request.json ?? "<NONE>"} and {request.parameters}");
            try {
                var result = _dispatcher.Call(
                    request.topic,
                    request.verb,
                    request.action,
                    request.json,
                    request.parameters.Select(parameter => new QueryParameter {
                        Name = parameter.name,
                        Value = parameter.value
                    }).ToList());

                return (ExecutionResponse)result;
            }
            catch (Exception error) {
                var rootError = GetRootError(error);
                _logger.LogError(rootError, $"execution of {request.topic}::{request.action} failed");
                return new ExecutionResponse {
                    code = 500,
                    errors = new[] {
                        new Error {
                            property = "Exception",
                            description = ConcatErrorMessages(error)
                        }
                    }
                };
            }
        }

        Exception GetRootError(Exception error)
        {
            var result = error;
            while (result.InnerException != null) result = result.InnerException;

            return result;
        }

        string ConcatErrorMessages(Exception error)
        {
            var errors = new List<string>();
            while (error != null)
            {
                errors.Add(error.Message);
                error = error.InnerException;
            }

            return string.Join(Environment.NewLine, errors);
        }
    }
}
