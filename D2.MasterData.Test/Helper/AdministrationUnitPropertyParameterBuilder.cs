using D2.Infrastructure;
using D2.MasterData.Models;
using D2.MasterData.Parameters;
using System;
using System.Collections.Generic;
using System.Text;

namespace D2.MasterData.Test.Helper
{
    public class AdministrationUnitPropertyParameterBuilder
    {
        private string _title;
        private string _description;
        private Variant _value;
        
        private AdministrationUnitPropertyParameterBuilder()
        {
            _title = "Wohnflaeche";
            _description = "Wird fuer Makleranzeigen benoetigt";
            _value = new Variant(new TypedValue(256,"meter", 3));
        }

        static public AdministrationUnitPropertyParameterBuilder New
        {
            get { return new AdministrationUnitPropertyParameterBuilder(); }
        }

        public AdministrationUnitPropertyParameters Build()
        {
            return new AdministrationUnitPropertyParameters
            {
                Title = _title,
                Description = _description,
                Value = _value
            };
        }
    }
}
