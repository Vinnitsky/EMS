using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoaW.CMS.Model.Persons;

namespace WoaW.CMS.DAL.EF.Configurations
{
    class PartyIdentityConfiguration : EntityTypeConfiguration<PartyIdentity>
    {
        public PartyIdentityConfiguration()
        {
            ToTable("AspNetUsers");
            HasRequired(t => t.Party);
        }
    }
}
