using System.Data.Entity.ModelConfiguration;
using WoaW.CRM.Model;
using WoaW.CRM.Model.Repationships;

namespace WoaW.CRM.DAL.EF.Configurations
{
    sealed class RuleSetConfiguration : EntityTypeConfiguration<RuleSet>
    {
        public RuleSetConfiguration()
        {
            ToTable("RuleSet");
        }
    }
}
