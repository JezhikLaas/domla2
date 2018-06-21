using D2.MasterData.Infrastructure;
using D2.MasterData.Infrastructure.Validation;
using System;
using System.Collections.Generic;
using D2.Common;

namespace D2.MasterData.Parameters
{
    public class AdministrationUnitParameters
    {
        public Guid Id { get; set; }

        [NotNullOrEmpty(RequestType.Put, RequestType.Post, RequestType.Patch)]
        public string UserKey { get; set; }

        [NotNullOrEmpty]
        public string Title { get; set; }

        [NotNullOrEmpty(RequestType.Put, RequestType.Post)]
        public List<EntranceParameters> Entrances { get; set; }

        public YearMonth? YearOfConstruction { get; set; }

        public int Version { get; set; }

        public List<AdministrationUnitPropertyParameters> AdministrationUnitProperties { get; set; }
    }
}
