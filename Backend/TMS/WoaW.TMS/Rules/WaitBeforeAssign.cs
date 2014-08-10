using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoaW.NS;

namespace WoaW.TMS.Tasks.Rules
{
    public class WaitBeforeAssign : BaseTimeValidator
    {
        private INotificationCenter _incidentManager;

        public WaitBeforeAssign(INotificationCenter incidentManager)
        {
            _incidentManager = incidentManager;
            Title = "rule1";
            ApplyForStatus = EWorkEffortStatus.Created;
        }
        protected virtual void Validate(ObservableCollection<INotification> notifications, ResourceManager manager, WorkEffort effort)
        {
            var p1 = manager.Assignments.SingleOrDefault(a => a.WorkEffort == effort);
            var p2 = manager.WorkEfforts.Contains(effort);

            if (p1 == null && p2 == true)
            {
                var notification = new Notification(ENotificationType.Allert);
                notification.Description = "alerts";
                notifications.Add(notification);
            }
        }

        public override void Setup(ResourceManager manager, IList<WorkEffort> list)
        {
            var tasks = from t in list orderby t.Priority select t;
            foreach (var task in tasks)
            {
                System.Threading.Tasks.Task.Delay(manager.MaxTimeBeforeAssignTask)
                    .ContinueWith(t => Validate(_incidentManager.Notifications, manager, task));
            }
        }

        public override void Setup(ResourceManager manager, IList<WorkEffortPartyAssignment> list)
        {
            throw new NotImplementedException();
        }
    }
}
