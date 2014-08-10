using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WoaW.TMS.Tasks
{
    public class WorkEffortAssociationByType : IWorkEffortAssociation
    {
        #region properties
        public Type TypeOfAssociatedWorkEffort { get; private set; }
        #endregion

        #region cobstructors
        public WorkEffortAssociationByType(Type type)
        {
            TypeOfAssociatedWorkEffort = type;
        }
        #endregion

        public bool IsAssociated(object item)
        {
            return item.GetType() == TypeOfAssociatedWorkEffort;
        }
    }
}
