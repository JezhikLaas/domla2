using D2.MasterData.Parameters;
using Xunit;

namespace D2.MasterData.Test
{
    public class AdministrationUnitParameterValidationTest
    {
        [Fact]
        public void ValidatePostRequest()
        {
            var parameters = new AdministrationUnitParameters {
                Title = "ABC",
                UserKey = "02",
                Address = new AddressParameters {
                    City = "H",
                    Country = new CountryInfoParameters {
                        Iso2 = "DE",
                        Name = "Deutschland",
                        Iso3 = "DEU"
                    }
                }
            };
        }
    }
}
