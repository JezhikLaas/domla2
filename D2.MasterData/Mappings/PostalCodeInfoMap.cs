using D2.MasterData.Models;
using FluentNHibernate.Mapping;

namespace D2.MasterData.Mappings
{
    public class PostalCodeInfoMap : ClassMap<PostalCodeInfo>
    {
        public PostalCodeInfoMap()
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
                .Length(20)
                .Not
                .Nullable();
            Map(x => x.Edit)
                .Access.BackingField()
                .Generated.Always();
            Map(x => x.City)
                .Access.BackingField()
                .Length(256)
                .Not.Nullable();
            Version(x => x.Version)
                .Access.BackingField()
                .Generated.Never()
                .Not.Nullable();
            OptimisticLock.Version();
        }
    }
}
