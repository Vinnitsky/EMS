namespace WoaW.CRM.DAL.EF.IdentityMigration
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class IdentityConfiguration : DbMigrationsConfiguration<WoaW.CRM.DAL.EF.UserDbContext>
    {
        public IdentityConfiguration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"IdentityMigration";
        }

        protected override void Seed(WoaW.CRM.DAL.EF.UserDbContext context)
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
