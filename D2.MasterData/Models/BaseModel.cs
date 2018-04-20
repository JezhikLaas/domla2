using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace D2.MasterData.Models
{
    public class BaseModel
    {
        protected Guid _id;
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id
        {
            get { return _id; }
            internal set { _id = value; }
        }

        protected DateTime _edit;
        public DateTime Edit
        {
            get { return _edit; }
            private set { _edit = value; }
        }
    }
}
