using System.Data.Entity.ModelConfiguration;
using WoaW.CMS.Model;
using WoaW.CMS.Model.Clasifications;

namespace WoaW.CMS.DAL.EF.Configurations
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
