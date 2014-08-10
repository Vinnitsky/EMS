using System.Data.Entity.ModelConfiguration;
using WoaW.CMS.Model;
using WoaW.CMS.Model.Repationships;

namespace WoaW.CMS.DAL.EF.Configurations
{
    sealed class RuleSetConfiguration : EntityTypeConfiguration<RuleSet>
    {
        public RuleSetConfiguration()
        {
            ToTable("RuleSet");
        }
    }
}
