using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoaW.CRM.DAL.EF.Configurations;

namespace WoaW.CRM.DAL.EF
{
    public class CrmDbContext : DbContext
    {
        public CrmDbContext()
            : base("DefaultConnection")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();

            new CRMMapper(modelBuilder);
        }
 
        public void Seed(CrmDbContext context)
        {
#if DEBUG
            new CRMSeed(context);
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
