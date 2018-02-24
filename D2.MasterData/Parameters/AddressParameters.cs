using D2.MasterData.Infrastructure.Validation;
using D2.MasterData.Models;

namespace D2.MasterData.Parameters
{
    public class AddressParameters
    {
        [NotNullOrEmpty]
        public string Street { get; set; }

        [NotNullOrEmpty]
        public string Number { get; set; }

        [NotNullOrEmpty]
        public CountryInfoParameters Country { get; set; }

        [NotNullOrEmpty]
        public string PostalCode { get; set; }

        [NotNullOrEmpty]
        public string City { get; set; }
    }
}
