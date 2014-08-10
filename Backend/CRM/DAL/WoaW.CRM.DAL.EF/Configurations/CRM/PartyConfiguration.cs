using System.Data.Entity.ModelConfiguration;
using WoaW.CMS.Model;

namespace WoaW.CMS.DAL.EF.Configurations
{
    sealed class PartyConfiguration : EntityTypeConfiguration<Party>
    {
        public PartyConfiguration()
        {
            ToTable("Party");
            Property(t => t.Title).IsRequired();
            Property(t => t.FromDate).HasColumnType("datetime2");
            Property(t => t.ThruDate).HasColumnType("datetime2");
        }
    }
}
