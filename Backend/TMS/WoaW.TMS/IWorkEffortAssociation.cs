using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WoaW.TMS.Tasks
{
    public interface IWorkEffortAssociation
    {
        bool IsAssociated(object item);
    }
}
