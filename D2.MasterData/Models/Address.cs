using D2.MasterData.Parameters;

namespace D2.MasterData.Models
{
    public class Address
    {
        private Address()
        { }

        public Address(AddressParameters argument)
        {
            Street = argument.Street;
            Number = argument.Number;
            PostalCode = argument.PostalCode;
            City = argument.City;
            Country = new CountryInfo(argument.Country);
        }

        public string Street
        {
            get;
            private set;
        }

        public string Number
        {
            get;
            private set;
        }

        public CountryInfo Country
        {
            get;
            private set;
        }

        public string PostalCode
        {
            get;
            private set;
        }

        public string City
        {
            get;
            private set;
        }
    }
}
