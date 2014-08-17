using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoaW.TMS.Model.DAL.Configuration
{
    public class TimeSpanNullConvention : Convention
    {
        public TimeSpanNullConvention()
        {
            this.Properties<TimeSpan?>().Configure(c => c.HasColumnType("time(0)"));
        }
    }
    public class TimeSpanConvention : Convention
    {
        public TimeSpanConvention()
        {
            this.Properties<TimeSpan>().Configure(c => c.HasColumnType("time(0)"));
        }
    }

    public class DateTime2NullConvention : Convention
    {
        public DateTime2NullConvention()
        {
            this.Properties<DateTime?>().Configure(c => c.HasColumnType("datetime2"));
        }
    }
    public class DateTime2Convention : Convention
    {
        public DateTime2Convention()
        {
            this.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));
        }
    }
}
