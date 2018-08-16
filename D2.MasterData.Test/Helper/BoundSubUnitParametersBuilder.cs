using D2.MasterData.Parameters;
using System;
using System.Collections.Generic;
using System.Text;

namespace D2.MasterData.Test.Helper
{
    class BoundSubUnitParametersBuilder
    {
        private string _title;
        private int _number;
        private string _usage;
        private string _floor;

        private BoundSubUnitParametersBuilder ()
        {
            _title = "Wohnung1";
            _number = 1;
            _usage = "Wohnung";
            _floor = "EG";

        }

        static public BoundSubUnitParametersBuilder New
        {
            get { return new BoundSubUnitParametersBuilder(); }
        }

        public BoundSubUnitParameters Build()
        {
            return new BoundSubUnitParameters
            {
                Title = _title,
                Number = _number,
                Usage = _usage,
                Floor = _floor
            };
        }
    }
}
