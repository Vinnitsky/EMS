using System.Data.Entity.ModelConfiguration;

namespace WoaW.TMS.Model.DAL.Configuration
{
    public sealed class WorkEffortHistorycalRecordConfiguration : EntityTypeConfiguration<WorkEffortHistorycalRecord>
    {
        public WorkEffortHistorycalRecordConfiguration()
        {
            ToTable("WorkEffortHistorycalRecord").HasKey(t => t.Id);

            Property(t => t.TaskId).IsRequired();
            Property(t => t.EmployeeId).IsOptional();
            Property(t => t.ManagerId).IsOptional();
            Property(t => t.Description).IsOptional();
            Property(t => t.Time).IsRequired();
            Property(t => t.Status).IsOptional();
        }
    }
}
