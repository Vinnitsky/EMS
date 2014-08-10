using System.Data.Entity.ModelConfiguration;
using WoaW.CRM.Model;
using WoaW.CRM.Model.Clasifications;

namespace WoaW.CRM.DAL.EF.Configurations
{
    sealed class PartyClasificationConfiguration : EntityTypeConfiguration<PartyClasification>
    {
        public PartyClasificationConfiguration()
        {
            ToTable("PartyClasification");
            //this.HasKey(t => t.Id);
            Property(t => t.FromDate).HasColumnType("datetime2");
            Property(t => t.ThruDate).HasColumnType("datetime2");

        }
    }
}
