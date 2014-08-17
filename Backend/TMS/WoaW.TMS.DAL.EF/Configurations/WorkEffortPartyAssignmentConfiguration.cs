using System.Data.Entity.ModelConfiguration;

namespace WoaW.TMS.Model.DAL.Configuration
{
    public sealed class WorkEffortPartyAssignmentConfiguration : EntityTypeConfiguration<WorkEffortPartyAssignment>
    {
        public WorkEffortPartyAssignmentConfiguration()
        {
            ToTable("WorkEffortPartyAssignment").HasKey(t => t.Id);

            Property(t => t.Status).IsRequired();
            Property(t => t.CreatedAt).IsRequired();
            Property(t => t.AssignedAt).IsOptional();
            Property(t => t.AcceptedAt).IsOptional();
            Property(t => t.RejectedAt).IsOptional();
            Property(t => t.ClosedAt).IsOptional();
            HasOptional(t => t.AssignedTo);
            HasRequired(t => t.WorkEffort);

            HasMany<WorkEffortHistorycalRecord>(t=>t.History);
        }
    }
}
