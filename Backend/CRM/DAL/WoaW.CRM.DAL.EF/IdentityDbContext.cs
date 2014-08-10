using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using WoaW.CMS.DAL.EF.Configurations;

namespace WoaW.CMS.DAL.EF
{
    public class UserDbContext : IdentityDbContext//<IdentityUser>
    {
        public UserDbContext()
            : base("Users")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();

            new IdentityMapper(modelBuilder);
        }

        public void Seed(UserDbContext context)
        {
#if DEBUG
            new IdentitySeed(context);
#endif
            // Normal seeding here
            context.SaveChanges();
        }

        public class DropCreateDatabaseAlwaysInitializer : DropCreateDatabaseAlways<UserDbContext>
        {
            protected override void Seed(UserDbContext context)
            {
                context.Seed(context);

                base.Seed(context);
            }
        }

        public class DropCreateIfChangeInitializer : DropCreateDatabaseIfModelChanges<UserDbContext>
        {
            protected override void Seed(UserDbContext context)
            {
                context.Seed(context);

                base.Seed(context);
            }
        }

        public class CreateInitializer : CreateDatabaseIfNotExists<UserDbContext>
        {
            protected override void Seed(UserDbContext context)
            {
                context.Seed(context);

                base.Seed(context);
            }
        }

        static UserDbContext()
        {
#if DEBUG
            Database.SetInitializer<UserDbContext>(new DropCreateDatabaseAlwaysInitializer());
#endif
        }
    }
}
