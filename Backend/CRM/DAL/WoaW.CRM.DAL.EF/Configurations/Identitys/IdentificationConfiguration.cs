using System.Data.Entity.ModelConfiguration;
using WoaW.CRM.Model.Identities;

namespace WoaW.CRM.DAL.EF.Configurations
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
