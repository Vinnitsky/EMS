using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoaW.CMS.Model.Persons;

namespace WoaW.CMS.DAL.EF.Configurations
{
    sealed class PersonConfiguration : EntityTypeConfiguration<Person>
    {
        public PersonConfiguration()
        {
            ToTable("Person");
            Property(t => t.Birthdate).HasColumnType("datetime2");

        }
    }
}
