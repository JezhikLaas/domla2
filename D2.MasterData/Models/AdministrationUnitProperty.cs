using D2.Infrastructure;
using D2.MasterData.Parameters;
using System;

namespace D2.MasterData.Models
{
    public class AdministrationUnitProperty : BaseModel
    {
        protected AdministrationUnitProperty()
        { }

        public AdministrationUnitProperty(AdministrationUnitPropertyParameters argument, AdministrationUnit unit)
        {
            Id = argument.Id;
            Titel = argument.Title;
            Description = argument.Description;
            Value = argument.Value;
            Version = argument.Version;
            AdministrationUnitId = unit.Id;
        }

        public virtual Guid AdministrationUnitId
        {
            get;
            protected set;
        }
        public virtual string Titel
        {
            get;
            protected set;
        }

        public virtual string Description
        {
            get;
            protected set;
        }

        public virtual Variant Value
        {
            get;
            protected set;
        }

        public virtual int Version
        {
            get;
            protected set;
        }
    }
}
