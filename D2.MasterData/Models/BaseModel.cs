using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace D2.MasterData.Models
{
    public class BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
