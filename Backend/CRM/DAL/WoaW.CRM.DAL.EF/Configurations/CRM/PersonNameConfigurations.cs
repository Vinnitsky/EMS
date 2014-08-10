using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoaW.CMS.Model.Persons;

namespace WoaW.CMS.DAL.EF.Configurations
{
    class PersonNameConfigurations : EntityTypeConfiguration<PersonName>
    {
        public PersonNameConfigurations()
        {
            Property(t => t.Name).IsRequired();
            Property(t => t.FromDate).HasColumnType("datetime2");
            Property(t => t.ThruDate).HasColumnType("datetime2");
        }
    }
    class PersonNameOptionConfigurations : EntityTypeConfiguration<PersonNameOption>
    {
        public PersonNameOptionConfigurations()
        {
            Property(t => t.Value).IsRequired();
        }
    }
    class PersonNameTypeConfigurations : EntityTypeConfiguration<PersonNameType>
    {
        public PersonNameTypeConfigurations()
        {
            Property(t => t.Title).IsRequired();
        }
    }
}
