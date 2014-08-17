using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoaW.NS;

namespace WoaW.TMS.Model.Rules
{
    public class WaitBeforeAccept : BaseTimeValidator
    {
        private INotificationCenter _incidentManager;

        public WaitBeforeAccept(INotificationCenter incidentManager)
        {
            _incidentManager = incidentManager;
            Title = "rule1";
            ApplyForStatus = EWorkEffortStatus.Assigned;
        }
        protected virtual void ValidateWaitingTime(ObservableCollection<INotification> notifications, WorkEffortPartyAssignment assignment)
        {
            if (assignment.Status != EWorkEffortStatus.Accepted || assignment.Status != EWorkEffortStatus.OnHold)
            {
                var notification = new Notification(ENotificationType.Allert);
                notification.Description = "alerts";
                notifications.Add(notification);
            }

        }


        public override void Setup(ResourceManager manager, IList<WorkEffort> list)
        {
            throw new NotImplementedException();
        }

        public override void Setup(ResourceManager manager, IList<WorkEffortPartyAssignment> list)
        {
            var assognments = from t in list orderby t.WorkEffort.Priority select t;
            foreach (var assignment in assognments)
            {
                System.Threading.Tasks.Task.Delay(manager.MaxTimeBeforeAcceptTask)
                    .ContinueWith(t => ValidateWaitingTime(_incidentManager.Notifications, assignment));
            }
        }
    }
}
