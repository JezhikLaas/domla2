using System;

namespace D2.MasterData.Models
{
    public abstract class BaseModel
    {
        public virtual Guid Id
        {
            get;
            protected set;
        }

        public virtual DateTime Edit
        {
            get;
            protected set;
        }
    }
}
