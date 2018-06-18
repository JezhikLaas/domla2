using D2.MasterData.Parameters;

namespace D2.MasterData.Test.Helper
{
    class PostalCodeInfoParametersBuilder
    {
        private string _postalCode;
        private string _iso2;
        private string _city;

        private PostalCodeInfoParametersBuilder()
        {
            _postalCode = "32602";
            _city = "Vlotho";
            _iso2 = "DE";
        }

        static public PostalCodeInfoParametersBuilder New
        {
            get { return new PostalCodeInfoParametersBuilder(); }
        }

        public PostalCodeInfoParameters Build()
        {
            return new PostalCodeInfoParameters
            {
                PostalCode = _postalCode,
                City = _city,
                Iso2 = _iso2
            };
        }
    }
}
