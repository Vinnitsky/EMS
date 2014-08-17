using System.Data.Entity;

namespace WoaW.TMS.Model.DAL.Configuration
{
    public sealed class TmsMapper
    {
        public TmsMapper(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();

            //modelBuilder.Configurations.Add(new WorkEffortConfiguration());
            modelBuilder.Configurations.Add(new TaskConfiguration());
            modelBuilder.Configurations.Add(new WorkEffortHistorycalRecordConfiguration());
            modelBuilder.Configurations.Add(new WorkEffortPartyAssignmentConfiguration());
            modelBuilder.Configurations.Add(new WorkEffortTypeConfiguration());

            modelBuilder.Conventions.Add(new TimeSpanNullConvention());
            //modelBuilder.Conventions.Add(new TimeSpanConvention());
            modelBuilder.Conventions.Add(new DateTime2NullConvention());
            modelBuilder.Conventions.Add(new DateTime2Convention());

            //modelBuilder.Types().Configure(c =>
            //{
            //    //NB the syntax used here will do this for all entities with a private isActive property
            //    var properties = c.ClrType.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance).Where(p => p.Name == "isActive");
            //    foreach (var p in properties)
            //        c.Property(p).HasColumnName("IsActive");
            //});
        }
    }
}
