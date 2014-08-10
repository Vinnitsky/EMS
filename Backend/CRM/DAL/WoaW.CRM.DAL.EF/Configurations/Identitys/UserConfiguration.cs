using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoaW.CRM.Model.Persons;

namespace WoaW.CRM.DAL.EF.Configurations
{
    class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            HasKey(t=>t.Id);
            HasMany<PartyIdentity>(t => t.Identities).WithRequired(t=>t.User).HasForeignKey(t=>t.UserId);

            //Property(t => t.Type).IsRequired();
            Property(t => t.FromDate).HasColumnType("datetime2");
            Property(t => t.ThruDate).HasColumnType("datetime2");
        }
    }
}
