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
    public class FindUserForTaskRule : BaseRule
    {
        private ResourceManager _resourceManager;

        public FindUserForTaskRule(ResourceManager resourceManager)
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

            // если задача уже назначена - выходим.
            if (manager.Assignments.Any(a => a.WorkEffort == effort))
                return null;

            // если нет свободных пользователей и таких что умеют выполнять требуемую роль - выходим
            //var users = from u in manager.Persons where u.IsBussy == false && u.PlayRoles.Contains(effort.RequerdRole) == true select u;
            var users = from u in manager.Persons
                        where u.IsBussy == false && u.Type == effort.RequerdRole
                        select u;
            if (users.Count() == 0)
                return null;

            var user = users.FirstOrDefault();
            return manager.AssignTask(user, effort);
        }
    }
}
