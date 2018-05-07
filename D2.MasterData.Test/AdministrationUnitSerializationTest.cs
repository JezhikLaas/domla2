using D2.MasterData.Parameters;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace D2.MasterData.Test
{
    public class AdministrationUnitSerializationTest
    {
        [Fact(DisplayName = "Serialize and deserialize AdministrationUnitParameters")]
        public void Serialize_and_deserialize_AdministrationUnitParameters()
        {
            var test = new AdministrationUnitParameters
            {
                UserKey = "02",
                Title = "ABC",
                Entrances = new List<EntranceParameters> {
                    new EntranceParameters {
                        Title = "Eingang 49",
                        Address = new AddressParameters {
                            Street = "Seumestraße",
                            Number = "49",
                            PostalCode = "22222",
                            Country = new CountryInfoParameters{
                                Iso2 = "DE",
                                Name = "Deutschland"
                            }
                        }
                    }
                }
            };

            var text = JsonConvert.SerializeObject(test);
            var check = JsonConvert.DeserializeObject<AdministrationUnitParameters>(text);

            Assert.Equal(test.Title, check.Title);
            Assert.Equal(test.UserKey, check.UserKey);
            Assert.Equal(test.Entrances.First().Title, check.Entrances.First().Title);
            Assert.Equal(test.Entrances.First().Address.Street, check.Entrances.First().Address.Street);
            Assert.Equal(test.Entrances.First().Address.Number, check.Entrances.First().Address.Number);
            Assert.Equal(test.Entrances.First().Address.PostalCode, check.Entrances.First().Address.PostalCode);
            Assert.Equal(test.Entrances.First().Address.Country.Iso2, check.Entrances.First().Address.Country.Iso2);
            Assert.Equal(test.Entrances.First().Address.Country.Name, check.Entrances.First().Address.Country.Name);
        }
    }
}
