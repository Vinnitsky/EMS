namespace WoaW.CMS.DAL.EF.CrmMigration
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class CrmConfiguration : DbMigrationsConfiguration<WoaW.CMS.DAL.EF.CrmDbContext>
    {
        public CrmConfiguration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"CrmMigration";
        }

        protected override void Seed(WoaW.CMS.DAL.EF.CrmDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
