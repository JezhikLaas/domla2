using D2.MasterData.Infrastructure;
using D2.MasterData.Models;
using System;

namespace D2.MasterData.Parameters
{
    public class AdministrationUnitParameters : RequestParameters
    {
        public Guid Id { get; set; }

        [NotNullOrEmpty(RequestType.Put, RequestType.Post, RequestType.Patch)]
        public string UserKey { get; set; }

        [NotNullOrEmpty]
        public string Title { get; set; }

        public Address Address { get; set; }
    }
}
