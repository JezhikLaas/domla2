using D2.MasterData.Parameters;
using System;
using System.ComponentModel.DataAnnotations;

namespace D2.MasterData.Models
{
    public class SubUnit : BaseModel
    {
        protected SubUnit()
        { }

        public virtual string Title
        {
            get;
            protected set;
        }



        public virtual int? Number
        {
            get;
            protected set;
        }

        public virtual string Usage
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
