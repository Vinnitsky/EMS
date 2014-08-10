using System.Data.Entity.ModelConfiguration;
using WoaW.CRM.Model;
using WoaW.CRM.Model.Repationships;

namespace WoaW.CRM.DAL.EF.Configurations
{
    sealed class PartyRelationshipConfiguration : EntityTypeConfiguration<PartyRelationship>
    {
        public PartyRelationshipConfiguration()
        {
            ToTable("PartyRelationship");
        }
    }
}
