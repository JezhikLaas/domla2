using D2.MasterData.Parameters;
using D2.MasterData.Test.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace D2.MasterData.Test
{
    public class AdministrationUnitPropertySerializationTest
    {
        [Fact(DisplayName = "Serialize and deserialize AdministrationUnitPropertyParameters with TypedValue")]
        public void Serialize_and_deserialize_AdministrationUnitPropertyParameters_with_TypedValue()
        {
            var test = AdministrationUnitPropertyParameterBuilder.New.Build();

            var text = JsonConvert.SerializeObject(test);
            var check = JsonConvert.DeserializeObject<AdministrationUnitPropertyParameters>(text);

            Assert.Equal(test.Title, check.Title);
            Assert.Equal(test.Description, check.Description);
            Assert.Equal(test.Value.Number, check.Value.Number);
            Assert.Equal(test.Value.Raw, check.Value.Raw);
            Assert.Equal(test.Value.Tag, check.Value.Tag);
            Assert.Equal(test.Value.String, check.Value.String);
            Assert.Equal(test.Value.ToString(), check.Value.ToString());
        }

        [Fact(DisplayName = "Serialize and deserialize AdministrationUnitPropertyParameters with DateTime")]
        public void Serialize_and_deserialize_AdministrationUnitPropertyParameters_with_DateTime()
        {
            AdministrationUnitPropertyParameters test = AdministrationUnitPropertyParameterBuilder.New.Build();
            test.Value = new D2.Infrastructure.Variant(new DateTime(5000000));
            var text = JsonConvert.SerializeObject(test);
            var check = JsonConvert.DeserializeObject<AdministrationUnitPropertyParameters>(text);

            Assert.Equal(test.Value.Raw, check.Value.Raw);
            Assert.Equal(test.Value.Tag, check.Value.Tag);
            Assert.Equal(test.Value.DateTime, check.Value.DateTime);
        }

        [Fact(DisplayName = "Serialize and deserialize AdministrationUnitPropertyParameters with String")]
        public void Serialize_and_deserialize_AdministrationUnitPropertyParameters_with_String()
        {
            AdministrationUnitPropertyParameters test = AdministrationUnitPropertyParameterBuilder.New.Build();
            test.Value = new D2.Infrastructure.Variant(new String("Hallo"));
            var text = JsonConvert.SerializeObject(test);
            var check = JsonConvert.DeserializeObject<AdministrationUnitPropertyParameters>(text);

            Assert.Equal(test.Value.Raw, check.Value.Raw);
            Assert.Equal(test.Value.Tag, check.Value.Tag);
            Assert.Equal(test.Value.String, check.Value.String);
            Assert.Equal(test.Value.ToString(), check.Value.ToString());
        }
    }
}

