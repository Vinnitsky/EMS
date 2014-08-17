using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoaW.TMS.Model.Rules
{
    /// <summary>
    /// метод должен проверять список задач и назаначать им сотрудников 
    /// котороые имеют такие же роли которые нужны для задачи
    /// </summary>
    public class FindUserForSameTaskRule : BaseRule
    {
        private ResourceManager _resourceManager;
        public FindUserForSameTaskRule(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }

        public override WorkEffortPartyAssignment Execute(ResourceManager manager, WorkEffort effort)
        {
            #region parameter validation
            if (manager == null)
                throw new ArgumentNullException("manager");
            if (manager.Assignments == null)
                throw new ArgumentNullException("manager.Assignments");
            if (effort == null)
                throw new ArgumentNullException("effort");
            #endregion

            if (effort.WorkEffortAssociations.Count == 0)
            {
                var rule = new FindUserForTaskRule(manager);
                return rule.Execute(manager, effort);
            }

            // если задача уже назначена - выходим.
            if (manager.Assignments.Any(a => a.WorkEffort == effort))
                return null;

            // если нет свободных пользователей и таких что умеют выполнять требуемую роль - выходим
            //var users2 = from u in manager.Persons where u.IsBussy == false && u.PlayRoles.Contains(effort.RequerdRole) == true 
            var users2 = from u in manager.Persons
                         where u.IsBussy == false && u.Type == effort.RequerdRole
                         && manager.Assignments.Any(a => a.Status == EWorkEffortStatus.Closed && a.AssignedTo == u)
                         select u;
            if (users2.Count() == 0)
                return null;

            // если есть свободные пользователи, 
            // то в этом случае необходимо провереть есть ли из свободных пользователей такой,
            // который уже делал залачу такого же типа  
            var users1 = (from a in manager.Assignments
                          where (a.WorkEffort != effort && a.AssignedTo != null)
                              && (effort.WorkEffortAssociations.Any(i => i.IsAssociated(a.WorkEffort) == true))
                          select a.AssignedTo).ToList();

            //если таких пользователей нет выходим
            if (users1.Count == 0)
            {
                var user = users2.FirstOrDefault();
                return manager.AssignTask(user, effort);

            }
            else
            {
                var user = users1.FirstOrDefault();
                return manager.AssignTask(user, effort);
            }
        }
    }
}
