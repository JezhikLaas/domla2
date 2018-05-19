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

        [Required]
        public string Iso2
        {
            get;
            private set;
        }

        [Required]
        public string Name
        {
            get;
            private set;
        }
    }
}
