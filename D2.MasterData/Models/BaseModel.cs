using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace D2.MasterData.Models
{
    public class BaseModel
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
