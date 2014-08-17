using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using WoaW.CMS.DAL.EF.Configurations;

namespace WoaW.Ems.Dal.EF
{
    public class CrmDbContext : IdentityDbContext
    {
        public CrmDbContext()
            : base("DefaultConnection")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();

            new CmsMapper(modelBuilder);
        }
 
        public void Seed(CrmDbContext context)
        {
#if DEBUG
            new CmsSeed(context);
#endif
            // Normal seeding here
            context.SaveChanges();
        }

        public class DropCreateDatabaseAlwaysInitializer : DropCreateDatabaseAlways<CrmDbContext> 
        {
            protected override void Seed(CrmDbContext context)
            {
                context.Seed(context);

                base.Seed(context);
            }
        }

        public class DropCreateIfChangeInitializer : DropCreateDatabaseIfModelChanges<CrmDbContext>
        {
            protected override void Seed(CrmDbContext context)
            {
                context.Seed(context);

                base.Seed(context);
            }
        }

        public class CreateInitializer : CreateDatabaseIfNotExists<CrmDbContext>
        {
            protected override void Seed(CrmDbContext context)
            {
                context.Seed(context);

                base.Seed(context);
            }
        }

        static CrmDbContext()
        {
#if DEBUG
            //Database.SetInitializer<MyDbContext>(new MigrateDatabaseToLatestVersion<MyDbContext, WoaW.CRM.DAL.EF.Migrations.Configuration>());
            //Database.SetInitializer<MyDbContext>(new DropCreateIfChangeInitializer());
            Database.SetInitializer<CrmDbContext>(new DropCreateDatabaseAlwaysInitializer());
#else
        Database.SetInitializer<MyDbContext> (new CreateInitializer ());
#endif
        }

    }
}
