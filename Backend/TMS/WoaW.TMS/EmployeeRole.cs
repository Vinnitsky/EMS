using System;
using System.Collections.Generic;
using WoaW.CRM.Model;
using WoaW.CRM.Model.Repationships;
using WoaW.TMS.Tasks;

namespace WoaW.TMS
{
    public class EmployeeRole : PartyRole
    {
        /// <summary>
        /// в том случае если выполнение текущей задачей было прервано босом
        /// то задача ставиться в этот стек и после того как сотрудник выполнил задачу
        /// назначенную руководителем, он должен продолжить выполнение этой задачи.
        /// </summary>
        public Stack<WorkEffortPartyAssignment> Tasks { get; set; }
        public List<WorkEffortRate> Rates { get; set; }
        public bool IsBussy { get; set; }
        public bool IsShared { get; set; }

        public EmployeeRole()
        {
            Rates = new List<WorkEffortRate>();
            Tasks = new Stack<WorkEffortPartyAssignment>();
        }
        public EmployeeRole(RoleType roleType, Party party, bool isShared = false)
            :base(roleType, party)
        {
            Rates = new List<WorkEffortRate>();
            Tasks = new Stack<WorkEffortPartyAssignment>();

        }
    }
}