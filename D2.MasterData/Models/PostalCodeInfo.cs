using D2.MasterData.Parameters;
using System.ComponentModel.DataAnnotations;


namespace D2.MasterData.Models
{
    public class PostalCodeInfo : BaseModel
    {
        protected PostalCodeInfo()
        { }

        public PostalCodeInfo(PostalCodeInfoParameters argument)
        {
            Id = argument.Id;
            Iso2 = argument.Iso2;
            PostalCode = argument.PostalCode;
            City = argument.City;
            Version = argument.Version;
        }

        [Required]
        public virtual string Iso2
        {
            get;
            protected set;
        }

        [Required]
        [MaxLength(20)]
        public virtual string PostalCode
        {
            get;
            protected set;
        }

        [Required]
        [MaxLength(256)]
        public virtual string City
        {
            get;
            protected set;
        }



        [ConcurrencyCheck]
        public virtual int Version
        {
            get;
            protected set;
        }

    }
}
