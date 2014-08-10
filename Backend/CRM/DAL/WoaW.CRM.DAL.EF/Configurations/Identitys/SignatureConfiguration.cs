using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoaW.CRM.Model.Identities;
using WoaW.CRM.Model.Persons;

namespace WoaW.CRM.DAL.EF.Configurations
{
    class SignatureConfiguration : EntityTypeConfiguration<Signature>
    {
        public SignatureConfiguration()
        {
            HasKey(t=>t.Id);

            Property(t => t.Image).IsRequired();
            Property(t => t.ValidFrom).HasColumnType("datetime2");
            Property(t => t.ValidTo).HasColumnType("datetime2");
        }
    }
}
