using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoaW.CMS.Model.Persons;

namespace WoaW.CMS.DAL.EF.Configurations
{
    class MaritalStatusTypeConfiguration: EntityTypeConfiguration<MaritalStatusType>
    {
        public MaritalStatusTypeConfiguration()
        {
            Property(t => t.Title).IsRequired();
        }
    }
}
