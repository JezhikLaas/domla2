using D2.MasterData.Parameters;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace D2.MasterData.Test
{
    public class PostalCodeInfoSerializationTest
    {
        [Fact(DisplayName = "Serialize and deserialize PostalCodeInfoParameters")]
        public void Serialize_and_deserialize_PostalCodeInfoParameters()
        {
            var test = new PostalCodeInfoParameters
            {
                PostalCode = "32051",
                City = "Herford",
            };

            var text = JsonConvert.SerializeObject(test);
            var check = JsonConvert.DeserializeObject<PostalCodeInfoParameters>(text);

            Assert.Equal(test.PostalCode, check.PostalCode);
            Assert.Equal(test.City, check.City);
        }
    }
}
