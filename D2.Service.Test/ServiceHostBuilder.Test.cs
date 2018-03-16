using D2.Service.Controller;
using D2.Service.ServiceProvider;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace D2.Service.Test
{
    public class ServiceHostBuilderTest
    {
        public class WorkingController : BaseController
        {
            public string Add(string firstWord, string secondWord)
            {
                return $"{firstWord} {secondWord}";
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
                var controller = (WorkingController)services.ResolveNamed<BaseController>("Working");
                if (controller.Add("hello", "world") != "hello world") throw new Exception("controller does not work");
            }
        }

        public class MinimalStartup
        {
            public void ConfigureServices(IServices services)
            { }

            public void Configure()
            { }
        }

        public class StartupWithConfigureParameters
        {
            public void ConfigureServices(IServices services)
            { }

            public void Configure(IConfiguration configuration)
            {
                if (configuration == null) throw new NullReferenceException("configuration is null");
            }
        }

        public class InvalidMinimalStartupOne
        {
            public void ConfigureServices()
            { }

            public void Configure()
            { }
        }

        public class InvalidMinimalStartupTwo
        {
            public void Configure()
            { }
        }

        public class InvalidMinimalStartupThree
        {
            public void ConfigureServices(IServices services)
            { }
        }

        [Fact(DisplayName = "Minimal service host build succeeds")]
        public void Minimal_build_succeeds()
        {
            var host = ServiceHost
                .CreateDefaultBuilder()
                .UseStartup<MinimalStartup>()
                .Build();
            Assert.NotNull(host);
        }

        [Fact(DisplayName = "Building a host w/o Startup fails")]
        public void Building_a_host_w_o_Startup_fails()
        {
            var builder = ServiceHost
                .CreateDefaultBuilder();
            Assert.Throws<InvalidOperationException>(() => builder.Build());
        }

        [Fact(DisplayName = "Building a host with Startup with invalid ConfigureServices fails")]
        public void Building_a_host_with_Startup_with_invalid_ConfigureServices_fails()
        {
            var builder = ServiceHost
                .CreateDefaultBuilder()
                .UseStartup<InvalidMinimalStartupOne>();
            Assert.Throws<MissingMethodException>(() => builder.Build());
        }

        [Fact(DisplayName = "Building a host with Startup with missing ConfigureServices fails")]
        public void Building_a_host_with_Startup_with_missing_ConfigureServices_fails()
        {
            var builder = ServiceHost
                .CreateDefaultBuilder()
                .UseStartup<InvalidMinimalStartupTwo>();
            Assert.Throws<MissingMethodException>(() => builder.Build());
        }

        [Fact(DisplayName = "Building a host with Startup with missing Configure fails")]
        public void Building_a_host_with_Startup_with_missing_Configure_fails()
        {
            var builder = ServiceHost
                .CreateDefaultBuilder()
                .UseStartup<InvalidMinimalStartupThree>();
            Assert.Throws<MissingMethodException>(() => builder.Build());
        }

        [Fact(DisplayName = "Parameters for Configure are supplied")]
        public void Parameters_for_Configure_are_supplied()
        {
            var host = ServiceHost
                .CreateDefaultBuilder()
                .UseStartup<StartupWithConfigureParameters>()
                .Build();
            Assert.NotNull(host);
        }
        
        [Fact(DisplayName = "Controllers can be registered automatically")]
        public void Controllers_can_be_registered_automatically()
        {
            var host = ServiceHost
                .CreateDefaultBuilder()
                .UseStartup<Startup>()
                .Build();
            Assert.NotNull(host);
        }
        
        [Fact(DisplayName = "Configuration can be modified")]
        public void Configuration_can_be_modified()
        {
            var host = ServiceHost
                .CreateDefaultBuilder()
                .UseStartup<Startup>()
                .UseConfiguration(builder => builder.AddInMemoryCollection(new[] { KeyValuePair.Create("one", "1") }))
                .Build();
            Assert.NotNull(host);
        }
    }
}
