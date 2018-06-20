using D2.Infrastructure;
using D2.MasterData.Parameters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace D2.MasterData.Models
{
    public class AdministrationUnitFeature : BaseModel
    {
        protected AdministrationUnitFeature ()
        { }

        public AdministrationUnitFeature (AdministrationUnitFeatureParameters argument)
        {
            Id = argument.Id;
            Version = argument.Version;
            Title = argument.Title;
            Description = argument.Description;
            Tag = argument.Tag;
            TypedValueDecimalPlace = argument.TypedValueDecimalPlace;
            TypedValueUnit = argument.TypedValueUnit;
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

        public virtual VariantTag Tag
        {
            get;
            protected set;
        }

        public virtual int TypedValueDecimalPlace
        {
            get;
            protected set;
        }

        public virtual string TypedValueUnit
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
