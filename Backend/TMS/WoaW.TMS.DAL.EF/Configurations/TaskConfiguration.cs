using System.Data.Entity.ModelConfiguration;

namespace WoaW.TMS.Model.DAL.Configuration
{
    public sealed class TaskConfiguration : EntityTypeConfiguration<Task>
    {
        public TaskConfiguration()
        {
            ToTable("WorkEffort").HasKey(t => t.Id);

            HasMany(t => t.WorkEffortAssociations);
            HasOptional(t => t.RequerdRole);
            HasOptional(t => t.AssignedToParty);
            HasRequired(t => t.Type);
            Property(t => t.Priority).IsRequired();
            Property(t => t.ActualTime).IsOptional();
            Property(t => t.EstimatedTime).IsOptional();

            //HasMany<WorkEffortHistorycalRecord>(t => t.History);
        }
    }
}
