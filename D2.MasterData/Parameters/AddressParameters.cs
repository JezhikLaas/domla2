using D2.MasterData.Models;

namespace D2.MasterData.Parameters
{
    public class AddressParameters
    {
        public string Street { get; set; }

        public string Number { get; set; }

        public CountryInfoParameters Country { get; set; }

        public string PostalCode { get; set; }

        public string City { get; set; }
    }
}
