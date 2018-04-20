using D2.MasterData.Models;
using FluentNHibernate.Mapping;

namespace D2.MasterData.Infrastructure
{
    public class AdministrationUnitMap : ClassMap<AdministrationUnit>
    {
        public AdministrationUnitMap()
        {
            Id(x => x.Id);
            Map(x => x.UserKey)
                .Access.BackingField()
                .Not
                .Nullable();
        }
    }
}