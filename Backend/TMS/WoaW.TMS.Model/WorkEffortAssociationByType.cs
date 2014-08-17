using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WoaW.TMS.Model
{
    public class WorkEffortAssociationByType : WorkEffortAssociation
    {
        #region properties
        public override bool IsAssociated(WorkEffort effort)
        {
            return effort.Type.Id == TypeOfAssociatedWorkEffort.Id;
        }
        virtual public WorkEffortType TypeOfAssociatedWorkEffort { get; set; }
        #endregion

        #region cobstructors
        public WorkEffortAssociationByType(WorkEffortType type)
        {
            TypeOfAssociatedWorkEffort = type;
        }
        #endregion

    }
}
