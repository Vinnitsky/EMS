using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoaW.TMS.Model.Rules
{
    public abstract class BaseTimeValidator
    {
        public string Title { get; set; }

        public EWorkEffortStatus ApplyForStatus { get; set; }

        public abstract void Setup(ResourceManager manager, IList<WorkEffort> list);
        public abstract void Setup(ResourceManager manager, IList<WorkEffortPartyAssignment> list);
    }
}
