using System;

namespace D2.MasterData.Models
{
    public struct CountryInfo
    {
        private string _iso2;
        private string _iso3;
        private string _name;

        public string Iso2
        {
            get => _iso2;
            set {
                if (value == null) throw new ArgumentException("Iso2 code must be a string of length 2");
                if (value.Length != 2) throw new ArgumentException("Iso2 code must be a string of length 2");
                _iso2 = value;
            }
        }

        public string Iso3
        {
            get => _iso3;
            set { _iso3 = value; }
        }
        public string Name
        {
            get => _name;
            set { _name = value; }
        }
    }
}
