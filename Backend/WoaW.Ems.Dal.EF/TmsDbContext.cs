using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using WoaW.CMS.DAL.EF.Configurations;
using WoaW.TMS.Model.DAL.Configuration;

namespace WoaW.Ems.Dal.EF
{
    public class EmsDbContext : IdentityDbContext
    {
        static EmsDbContext()
        {
#if DEBUG
            //Database.SetInitializer<MyDbContext>(new MigrateDatabaseToLatestVersion<MyDbContext, WoaW.CRM.DAL.EF.Migrations.Configuration>());
            //Database.SetInitializer<MyDbContext>(new DropCreateIfChangeInitializer());
            //Database.SetInitializer(new DropCreateDatabaseAlwaysInitializer());
#else
        Database.SetInitializer<MyDbContext> (new CreateInitializer ());
#endif
        }
        public EmsDbContext()
            : base("DefaultConnection")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();

            new CmsMapper(modelBuilder);
            new TmsMapper(modelBuilder);
        }

        public void Seed(DbContext context)
        {
#if DEBUG
            new CmsSeed(context);
            new TmsSeed(context);
#endif
            // Normal seeding here
            context.SaveChanges();
        }

        public class DropCreateDatabaseAlwaysInitializer : DropCreateDatabaseAlways<EmsDbContext>
        {
            protected override void Seed(EmsDbContext context)
            {
                context.Seed(context);

                base.Seed(context);
            }
        }

        public class DropCreateIfChangeInitializer : DropCreateDatabaseIfModelChanges<EmsDbContext>
        {
            protected override void Seed(EmsDbContext context)
            {
                context.Seed(context);

                base.Seed(context);
            }
        }

        public class CreateInitializer : CreateDatabaseIfNotExists<EmsDbContext>
        {
            protected override void Seed(EmsDbContext context)
            {
                context.Seed(context);

                base.Seed(context);
            }
        }


    }
}
