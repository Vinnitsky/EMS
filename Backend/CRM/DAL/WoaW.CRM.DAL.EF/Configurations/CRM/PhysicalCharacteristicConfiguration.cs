using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoaW.CMS.Model.Persons;

namespace WoaW.CMS.DAL.EF.Configurations
{
    class PhysicalCharacteristicConfiguration : EntityTypeConfiguration<PhysicalCharacteristic>
    {
        public PhysicalCharacteristicConfiguration()
        {
            Property(t => t.Value).IsRequired();
            Property(t => t.FromDate).HasColumnType("datetime2");
            Property(t => t.ThruDate).HasColumnType("datetime2");
        }
    }
    class PhisicalCharacteristicOptionConfiguration : EntityTypeConfiguration<PhisicalCharacteristicOption>
    {
        public PhisicalCharacteristicOptionConfiguration()
        {
            Property(t => t.Value).IsRequired();
        }
    }
}
