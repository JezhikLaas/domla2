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
            Title = argument.Title;
            Description = argument.Description;
            Value = argument.Value;
            Version = argument.Version;
            AdministrationUnit = unit;
        }

        public virtual AdministrationUnit AdministrationUnit
        {
            get;
            protected set;
        }

        public virtual string Title
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
