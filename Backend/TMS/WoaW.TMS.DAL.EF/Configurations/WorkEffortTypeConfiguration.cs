using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoaW.TMS;

namespace WoaW.TMS.Model.DAL.Configuration
{
    public sealed class WorkEffortTypeConfiguration : EntityTypeConfiguration<WorkEffortType>
    {
        public WorkEffortTypeConfiguration()
        {
            ToTable("WorkEffortType").HasKey(t => t.Id);
            Property(t => t.Title).IsRequired().HasMaxLength(256);
            Property(t => t.Description).IsOptional();

        }
    }
}
