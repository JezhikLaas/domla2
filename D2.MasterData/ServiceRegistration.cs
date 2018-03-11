using D2.Common;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace D2.MasterData
{
    public static class ServiceRegistration
    {
        public class EndPoint
        {
            public string Name { get; set; }
            public string Uri { get; set; }
        }

        public class Service
        {
            public string Name => "administrationunits";
            public string BaseUrl => ServiceConfiguration.configuration.Hosting.First().FullAddress;
            public string Group => "md";
            public int Version => ServiceConfiguration.versionInfo.Version;
            public int Patch => ServiceConfiguration.versionInfo.Patch;
            public EndPoint[] EndPoints => new[] {
                                   new EndPoint { Name = "list_administrationunits", Uri = "/AdministrationUnit/list" },
                                   new EndPoint { Name = "create_administrationunit", Uri = "/AdministrationUnit/create" }
                              };
        }

        public static bool registerSelf(ILogger logger)
        {
            var textData = JsonConvert.SerializeObject(new Service());

            return ServiceRegistrator.registerSelf(logger, textData);
        }
    }
}
