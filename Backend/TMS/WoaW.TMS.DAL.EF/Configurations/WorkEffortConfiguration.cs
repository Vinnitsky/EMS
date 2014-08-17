using System.Data.Entity.ModelConfiguration;

namespace WoaW.TMS.Model.DAL.Configuration
{
    public class WorkEffortConfiguration : EntityTypeConfiguration<WorkEffort>
    {
        public WorkEffortConfiguration()
        {
            ToTable("WorkEffort").HasKey(t => t.Id);

            HasMany(t => t.WorkEffortAssociations);
            HasOptional(t => t.RequerdRole);
            HasRequired(t => t.Type);
            Property(t => t.Priority).IsRequired();
            Property(t => t.ActualTime).IsOptional();
            Property(t => t.EstimatedTime).IsOptional();
        }
    }
}
