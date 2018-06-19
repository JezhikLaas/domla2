using D2.MasterData.Parameters;
using System.ComponentModel.DataAnnotations;

namespace D2.MasterData.Models
{
    public class CountryInfo
    {
        protected CountryInfo()
        { }

        public CountryInfo(CountryInfoParameters argument)
        {
            Iso2 = argument.Iso2;
            Name = argument.Name;
        }

        public string Iso2
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }
    }
}
