using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoaW.TMS.Tasks.Rules
{
    public abstract class BaseRule
    {
        public string Title { get; set; }

        public EWorkEffortStatus ApplyForStatus { get; set; }

        public abstract WorkEffortPartyAssignment Execute(ResourceManager manager, WorkEffort effort);
    }
}
