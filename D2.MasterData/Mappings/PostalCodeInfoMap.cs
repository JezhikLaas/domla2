using D2.MasterData.Models;
using FluentNHibernate.Mapping;

namespace D2.MasterData.Mappings
{
    public class PostalCodeInfoCreateMap : ClassMap<PostalCodeInfo>
    {
        public PostalCodeInfoCreateMap()
        {
            Table("postalcodeinfo");
            Id(x => x.Id);
            Schema("md");
            Map(x => x.Iso2)
                .Access.BackingField()
                .Length(3)
                .Not.Nullable();
            Map(x => x.PostalCode)
                .Access.BackingField()
                .Length(5)
                .Not
                .Nullable();
            Map(x => x.Edit)
                .Access.BackingField()
                .Generated.Always();
            Map(x => x.City)
                .Access.BackingField()
                .Length(256)
                .Not.Nullable();
        }
   }
    public class PostalCodeInfoMap : PostalCodeInfoCreateMap
    {
        public PostalCodeInfoMap()
        {
            Version(x => x.Version)
                .Column("xmin")
                .Generated.Always();
        }
    }
}
