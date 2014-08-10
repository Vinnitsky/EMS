using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoaW.CMS.Model.Organizations;

namespace WoaW.CMS.DAL.EF.Configurations
{
    sealed class OrganiazationConfiguration : EntityTypeConfiguration<Organization>
    {
        public OrganiazationConfiguration()
        {
            ToTable("Organization");
        }
    }
}
