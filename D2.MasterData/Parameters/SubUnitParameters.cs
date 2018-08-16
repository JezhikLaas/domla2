using D2.MasterData.Infrastructure;
using D2.MasterData.Infrastructure.Validation;
using System;

namespace D2.MasterData.Parameters
{
    public class SubUnitParameters
    {
        public Guid Id { get; set; }

        [NotNullOrEmpty]
        public string Title { get; set; }


        [NotNullOrEmpty(RequestType.Put, RequestType.Post)]
        public int?  Number { get; set; }

        public string Usage { get; set; }

        public int Version { get; set; }
    }
}
