using D2.Infrastructure;
using D2.MasterData.Models;
using D2.MasterData.Parameters;
using System;
using System.Collections.Generic;
using System.Text;

namespace D2.MasterData.Test.Helper
{
    class AdministrationUnitFeatureBuilder
    {
        readonly AdministrationUnitFeatureParameters _administrationUnitFeatureParameters;

        private AdministrationUnitFeatureBuilder()
        {
            _administrationUnitFeatureParameters = new AdministrationUnitFeatureParameters
            {
                Title = "Wohnflaeche",
                Description = "Wird fuer Makleranzeigen benoetigt",
                Tag = VariantTag.TypedValue,
                TypedValueDecimalPlace = 2,
                TypedValueUnit = "meter"
            };
        }

        static public AdministrationUnitFeatureBuilder New
        {
            get { return new AdministrationUnitFeatureBuilder(); }
        }

        public AdministrationUnitFeatureBuilder WithId(Guid id)
        {
            _administrationUnitFeatureParameters.Id = id;
            return this;
        }

        public AdministrationUnitFeatureBuilder WithVersion(int version)
        {
            _administrationUnitFeatureParameters.Version = version;
            return this;
        }

        public AdministrationUnitFeatureBuilder WithTitle(string title)
        {
            _administrationUnitFeatureParameters.Title = title;
            return this;
        }

        public AdministrationUnitFeature Build()
        {
            return new AdministrationUnitFeature(_administrationUnitFeatureParameters);
        }
    }
}
