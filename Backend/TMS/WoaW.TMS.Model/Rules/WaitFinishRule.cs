using System;
using System.Linq;
using WoaW.NS;

namespace WoaW.TMS.Model.Rules
{
    public class WaitFinishRule : BaseTimeValidator
    {
        private INotificationCenter _incidentManager;

        public WaitFinishRule(INotificationCenter incidentManager)
        {
            _incidentManager = incidentManager;
            Title = "rule1";
            ApplyForStatus = EWorkEffortStatus.Accepted;
        }

        //TODO:!!!!!
        protected virtual void ValidateWaitingTime(ResourceManager manager, WorkEffortPartyAssignment assignment)
        {
            var time = assignment.AssignedAt + manager.MaxTimeInExecuteTask;
            if (time < DateTime.Now && assignment.Status == EWorkEffortStatus.Accepted)
            //if (task.Task.Status == ETaskStatus.Executed)
            {
                var notification = new Notification(ENotificationType.Allert);
                notification.Description = string.Format("пользователь UserId:{0} выполняет задачу TaskId:{1} слишком долго. время  началоа работы пользователя={2}, запланированиение время {3}",
                    assignment.AssignedTo.Id, assignment.WorkEffort.Id, assignment.AssignedAt, manager.MaxTimeInExecuteTask);

                _incidentManager.Notifications.Add(notification);
            }
        }

        public override void Setup(ResourceManager manager, System.Collections.Generic.IList<WorkEffort> list)
        {
            throw new NotImplementedException();
        }

        public override void Setup(ResourceManager manager, System.Collections.Generic.IList<WorkEffortPartyAssignment> list)
        {
            var assignments = from t in list where t.AssignedTo != null orderby t.WorkEffort.Priority select t;
            foreach (var assignment in assignments)
            {
                System.Threading.Tasks.Task.Delay(manager.MaxTimeInExecuteTask)
                    .ContinueWith(t => ValidateWaitingTime(manager, assignment));
            }
        }
    }
}
