using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoaW.CMS.DAL.EF.Configurations
{
    class IdentityMapper
    {
        public IdentityMapper(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new IdentificationConfiguration());
            modelBuilder.Configurations.Add(new SignatureConfiguration());
        }
    }

}
