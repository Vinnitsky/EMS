using System.Data.Entity.ModelConfiguration;
using WoaW.CMS.Model;
using WoaW.CMS.Model.Repationships;

namespace WoaW.CMS.DAL.EF.Configurations
{
    sealed class PartyRelationshipConfiguration : EntityTypeConfiguration<PartyRelationship>
    {
        public PartyRelationshipConfiguration()
        {
            ToTable("PartyRelationship");
        }
    }
}
