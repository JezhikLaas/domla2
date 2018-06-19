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

        public virtual string Iso2
        {
            get;
            protected set;
        }

       public virtual string PostalCode
        {
            get;
            protected set;
        }

        public virtual string City
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
