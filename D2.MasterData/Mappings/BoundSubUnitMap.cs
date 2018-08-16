using D2.MasterData.Models;
using FluentNHibernate.Mapping;

namespace D2.MasterData.Mappings
{
    public class BoundSubUnitMap: SubclassMap<BoundSubUnit>
    {
        public BoundSubUnitMap()
        {
            Table("boundsubunits");
            Schema("md");
            KeyColumn("Id");
            Map(x => x.Floor)
                    .Access.BackingField()
                    .Nullable();
            References(x => x.Entrance)
                    .Access.BackingField()
                    .Cascade.SaveUpdate()
                    .Not.Nullable();
        }
    }
}
