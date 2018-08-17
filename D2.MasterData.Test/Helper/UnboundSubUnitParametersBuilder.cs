using D2.MasterData.Parameters;
using System;
using System.Collections.Generic;
using System.Text;

namespace D2.MasterData.Test.Helper
{
    class UnboundSubUnitParametersBuilder
    {
        private string _title;
        private int _number;
        private string _usage;
        private UnboundSubUnitType _type;
        private UnboundSubUnitParametersBuilder()
        {
            _title = "Wohnung1";
            _number = 1;
            _usage = "Wohnung";
            _type = UnboundSubUnitType.Antenna;

        }

        static public UnboundSubUnitParametersBuilder New
        {
            get { return new UnboundSubUnitParametersBuilder(); }
        }

        public UnboundSubUnitParameters Build()
        {
            return new UnboundSubUnitParameters
            {
                Title = _title,
                Number = _number,
                Usage = _usage,
                Type = _type
            };
        }
    }
}
