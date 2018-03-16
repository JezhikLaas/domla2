using D2.Service.ServiceProvider;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace D2.Service.Test
{
    public class Service
    {
        public class MinimalStartup
        {
            public void ConfigureServices(IServices services)
            { }

            public void Configure()
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
    }
}
