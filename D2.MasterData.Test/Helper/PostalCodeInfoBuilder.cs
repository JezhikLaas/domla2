using D2.MasterData.Models;
using D2.MasterData.Parameters;
using System;
using System.Collections.Generic;

namespace D2.MasterData.Test.Helper
{
    class PostalCodeInfoBuilder
    {
        readonly PostalCodeInfoParameters _postalCodeInfoParameters;

        private PostalCodeInfoBuilder()
        {
            _postalCodeInfoParameters = new PostalCodeInfoParameters
            {
                PostalCode = "32602",
                City = "Vlotho",
                Iso2 = "DE"
            };
        }

        static public PostalCodeInfoBuilder New
        {
            get { return new PostalCodeInfoBuilder(); }
        }

        public PostalCodeInfoBuilder WithId(Guid id)
        {
            _postalCodeInfoParameters.Id = id;
            return this;
        }

        public PostalCodeInfoBuilder WithVersion(int version)
        {
            _postalCodeInfoParameters.Version = version;
            return this;
        }

        public PostalCodeInfo Build()
        {
            return new PostalCodeInfo(_postalCodeInfoParameters);
        }
    }
}
