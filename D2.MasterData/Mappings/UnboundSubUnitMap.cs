using D2.MasterData.Models;
using FluentNHibernate.Mapping;

namespace D2.MasterData.Mappings
{
    public class UnboundSubUnitMap : SubclassMap<UnboundSubUnit>
    {
        public UnboundSubUnitMap()
        {
            Table("unboundsubunits");
            Schema("md");
            KeyColumn("Id");
            Map(x => x.Type)
                        .Access.BackingField()
                        .Nullable();
        }
    }
}
