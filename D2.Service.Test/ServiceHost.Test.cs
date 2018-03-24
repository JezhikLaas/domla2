using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using D2.Service.Contracts.Common;
using D2.Service.Contracts.Execution;
using D2.Service.Contracts.Validation;
using D2.Service.Controller;
using D2.Service.IoC;
using D2.Service.ServiceProvider;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace D2.Service.Test
{
    public class ServiceHostTest
    {
        public class AddController : BaseController
        {
            public ValidationResponse PostValidate_Add(string firstWord, string secondWord)
            {
                var errors = new List<Error>();
                if (firstWord == null) {
                    errors.Add(new Error(nameof(firstWord), "must not be null"));
                }
                if (secondWord == null) {
                    errors.Add(new Error(nameof(secondWord), "must not be null"));
                }

                return new ValidationResponse(
                    errors.Any() ? State.ExternalFailure : State.NoError,
                    errors.ToArray()
                );
            }

            public ExecutionResponse PostAdd(string firstWord, string secondWord)
            {
                return new ExecutionResponse(
                    0,
                    $"{firstWord} {secondWord}",
                    null
                );
            }
        }

        public class Startup
        {
            public void ConfigureServices(IServices services)
            {
                services.AddControllers();

            }

            public void Configure(IServices services)
            {
            }
        }

        public class TestClient
        {
            public bool RunValidation()
            {
                using (var communicator = Ice.Util.initialize()) {
                    Ice.ObjectPrx obj = communicator.stringToProxy("Validator:default -h 127.0.0.1 -p 10000");
                    ValidatorPrx validator = ValidatorPrxHelper.uncheckedCast(obj);
                    if (validator == null) throw new ApplicationException("invalid proxy");

                    var result = validator.validate(
                        new Request(
                            "Add",
                            "Post",
                            "Add",
                            null,
                            new[] {
                            new Parameter("firstWord", "Hello"),
                            new Parameter("secondWord", "world!")
                        })
                    );

                    return result.result == State.NoError;
                }
            }

            public string RunExecution()
            {
                using (var communicator = Ice.Util.initialize()) {
                    Ice.ObjectPrx obj = communicator.stringToProxy("Executor:default -h 127.0.0.1 -p 10000");
                    ExecutorPrx executor = ExecutorPrxHelper.uncheckedCast(obj);
                    if (executor == null) throw new ApplicationException("invalid proxy");

                    var result = executor.execute(
                        new Request(
                            "Add",
                            "Post",
                            "Add",
                            null,
                            new[] {
                            new Parameter("firstWord", "Hello"),
                            new Parameter("secondWord", "world!")
                        })
                    );

                    return result.json;
                }
            }
        }

        [Fact(DisplayName = "Service can be started and stopped")]
        public void Service_can_be_started_and_stopped()
        {
            var configuration = new Dictionary<string, string> {
                { "Ice:Dispatcher:Endpoints", "tcp -h 127.0.0.1 -p 9999" }
            };
            var host = ServiceHost
                .CreateDefaultBuilder()
                .UseStartup<Startup>()
                .UseConfiguration(
                    builder => builder.AddInMemoryCollection(configuration)
                )
                .Build();

            Task.Run(() => host.Run());

            while (host.ServiceReady == false) Thread.Yield();
            var service = host.Resolve<IService>();

            Thread.Sleep(100);

            service.Shutdown();
        }

        [Fact(DisplayName = "Service can be used")]
        public void Service_can_be_used()
        {
            var configuration = new Dictionary<string, string> {
                { "Ice:Dispatcher:Endpoints", "tcp -h 127.0.0.1 -p 10000" }
            };
            var host = ServiceHost
                .CreateDefaultBuilder()
                .UseStartup<Startup>()
                .UseConfiguration(
                    builder => builder.AddInMemoryCollection(configuration)
                )
                .Build();

            Task.Run(() => host.Run());

            while (host.ServiceReady == false) Thread.Yield();
            var service = host.Resolve<IService>();

            try {
                Thread.Sleep(100);
                var client = new TestClient();
                Assert.True(client.RunValidation());
                Assert.Equal("Hello world!", client.RunExecution());
            }
            finally {
                service.Shutdown();
            }
        }
    }
}
