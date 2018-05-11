using D2.MasterData.Infrastructure.Validation;

namespace D2.MasterData.Parameters
{
    public class CountryInfoParameters
    {
        [NotNullOrEmpty]
        public string Iso2 { get; set; }

        [NotNullOrEmpty]
        public string Name { get; set; }
    }
}
