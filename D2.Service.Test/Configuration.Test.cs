using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace D2.Service.Test
{
    public class ConfigurationTest
    {
        [Fact]
        public void Enumerate_nested_configuration()
        {
            var dict = new Dictionary<string, string>
            {
                {"Profile:MachineName", "Rick"},
                {"App:MainWindow:Height", "11"},
                {"App:MainWindow:Width", "11"},
                {"App:MainWindow:Top", "11"},
                {"App:MainWindow:Left", "11"}
            };

            var builder = new ConfigurationBuilder();
            var configuration = builder.AddInMemoryCollection(dict).Build();
            var collection = configuration
                .GetSection("App")
                .AsEnumerable()
                .Where(pair => pair.Value != null)
                .ToList();

            Assert.Equal(4, collection.Count);
            Assert.Contains(collection, pair => pair.Key == "App:MainWindow:Height");
        }
    }
}
