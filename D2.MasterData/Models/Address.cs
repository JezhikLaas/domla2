namespace D2.MasterData.Models
{
    public struct Address
    {
        public string Street { get; set; }
        public string Number { get; set; }
        public CountryInfo Country { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
    }
}
