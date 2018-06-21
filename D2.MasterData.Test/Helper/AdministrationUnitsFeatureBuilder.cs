using D2.Infrastructure;
using D2.MasterData.Models;
using D2.MasterData.Parameters;
using System;
using System.Collections.Generic;
using System.Text;

namespace D2.MasterData.Test.Helper
{
    class AdministrationUnitsFeatureBuilder
    {
        readonly AdministrationUnitsFeatureParameters _administrationUnitsFeatureParameters;

        private AdministrationUnitsFeatureBuilder()
        {
            _administrationUnitsFeatureParameters = new AdministrationUnitsFeatureParameters
            {
                Title = "Wohnflaeche",
                Description = "Wird fuer Makleranzeigen benoetigt",
                Tag = VariantTag.TypedValue,
                TypedValueDecimalPlace = 2,
                TypedValueUnit = "meter"
            };
        }

        static public AdministrationUnitsFeatureBuilder New
        {
            get { return new AdministrationUnitsFeatureBuilder(); }
        }

        public AdministrationUnitsFeatureBuilder WithId(Guid id)
        {
            _administrationUnitsFeatureParameters.Id = id;
            return this;
        }

        public AdministrationUnitsFeatureBuilder WithVersion(int version)
        {
            _administrationUnitsFeatureParameters.Version = version;
            return this;
        }

        public AdministrationUnitsFeatureBuilder WithTitle(string title)
        {
            _administrationUnitsFeatureParameters.Title = title;
            return this;
        }

        public AdministrationUnitsFeature Build()
        {
            return new AdministrationUnitsFeature(_administrationUnitsFeatureParameters);
        }
    }
}
