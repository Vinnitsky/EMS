using System.Data.Entity.ModelConfiguration;
using WoaW.CMS.Model.Identities;

namespace WoaW.CMS.DAL.EF.Configurations
{
    class IdentificationConfiguration : EntityTypeConfiguration<Identification>
    {
        public IdentificationConfiguration()
        {
            HasKey(t=>t.Id);

            Property(t => t.Title).IsRequired();
            Property(t => t.ValidFrom).HasColumnType("datetime2");
            Property(t => t.ValidTo).HasColumnType("datetime2");
        }
    }
}
