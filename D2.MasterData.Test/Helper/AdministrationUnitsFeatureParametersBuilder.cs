using D2.Infrastructure;
using D2.MasterData.Parameters;
using System;
using System.Collections.Generic;
using System.Text;

namespace D2.MasterData.Test.Helper
{
    class AdministrationUnitsFeatureParametersBuilder
    {
        private string _title;
        private string _description;
        private VariantTag _tag;
        private int _typedValueDecimalPlace;
        private string _typedValueUnit;


        private AdministrationUnitsFeatureParametersBuilder()
        {
            _title = "Wohnflaeche";
            _description = "Wird fuer Makleranzeigen benoetigt";
            _tag = VariantTag.TypedValue;
            _typedValueDecimalPlace = 2;
            _typedValueUnit = "meter";
        }

        static public AdministrationUnitsFeatureParametersBuilder New
        {
            get { return new AdministrationUnitsFeatureParametersBuilder(); }
        }

        public AdministrationUnitsFeatureParameters Build()
        {
            return new AdministrationUnitsFeatureParameters
            {
                Title = _title,
                Description = _description,
                Tag = _tag,
                TypedValueDecimalPlace = _typedValueDecimalPlace,
                TypedValueUnit = _typedValueUnit
            };
        }
    }
}
