using System;
using System.Collections.Generic;
using WoaW.CMS.Model.Repationships;
using WoaW.NS;

namespace WoaW.TMS.Tasks
{
    /// <summary>
    /// deinition: задача это неделимая атомарная еденица работы которую выполняет один человек
    /// </summary>
    /// <remarks>
    /// этот клас играет роль контейнера для задачи , потому что не все что происходит с задачей 
    /// должно отображаться в свойствах самой задачи.
    /// например, создали задачу ... назначили пользователю А, он заболел. 
    /// задачу могут переназначить пользователю Б. зачем задачи иметь представление кто ее выполняет
    /// но запись о том что задачу выполняют нужно иеметь - вот этот класс говорит о том что здачу 
    /// взяли, кто ее взял, когда ее взял и все остальное, что связано с историей выполнения задачи. 
    /// Информация о ходе выполнения задачи может быть полезной для анализа производительности как 
    /// самого процесса так и пользователя... а так же она может поспособствовать анализу эффективности 
    /// процесса если посмотреть что происходило с задачей на всем промежутке времени выполнения процесса.
    /// 
    /// этот класс может использоваться в качестве контейнера для таких представителей 
    /// WorkEffort как: Programm, Project, Phase, Task, Activity
    /// 
    /// </remarks>
    public class WorkEffort
    {
        #region attributes
        private INotificationCenter _notificationCenter;
        private dynamic _container;

        #endregion

        #region properties
        public string Id { get; private set; }
        public dynamic Container { get { return _container; } }
        /// <summary>
        /// позволяет или запрещает руками назначать эту задачу.
        /// </summary>
        public bool AllowManualAcceptance { get; set; }
        public TimeSpan? EstimatedTime { get; set; }
        /// <summary>
        /// актуальное время выполнения задачи. 
        /// расчитывается от момента начала выполнения (ActualStartDatetime) 
        /// и до ее окончания (ActualCompletionDatetime)
        /// </summary>
        public TimeSpan? ActualTime { get; set; }
        /// <summary>
        /// максимально возможное время когда задача может быть актуальна
        /// </summary>
        public TimeSpan? TotalHoursAllowed { get; set; }
        /// <summary>
        /// время когда запланирован старт
        /// </summary>
        public DateTime? ScheduledStartDate { get; set; }
        /// <summary>
        /// время когда сотрудник начал выполнять задачу
        /// в данной реализации считаем, что время принятия задачи 
        /// является актуальным временем начала задачи. 
        /// т.е. если принял - то начал выполнять
        /// </summary>
        public DateTime? ActualStartDatetime { get; set; }
        /// <summary>
        /// дата когда задача действительно была закрыта 
        /// Status.Csosed
        /// </summary>
        public DateTime? ActualCompletionDatetime { get; set; }
        /// <summary>
        /// может работать с непосредственно таким же свойством задачи
        /// </summary>
        public int Priority { get; set; }
        /// <summary>
        /// может работать с непосредственно таким же свойством задачи
        /// </summary>
        public RoleType RequerdRole { get; set; }
        /// <summary>
        /// в данной реализации список сожержит ссылки на ассоциированные 
        /// с данной задачи. в будущем можно создать класс WorkEffortAssociation
        /// который будет позволять ассоциировать задачи как по типу так и по ID 
        /// 
        /// сейчас это просто тип ассоциированной задачи
        /// </summary>
        public List<IWorkEffortAssociation> WorkEffortAssociations { get; set; }
        public WorkEffortType Type { get; set; }
        public List<string> Comments { get; set; }
        #endregion

        #region constructors
        public WorkEffort(INotificationCenter notificationCenter, WorkEffortType type, dynamic task = null, RoleType roleType = null)
        {
            Id = Guid.NewGuid().ToString();
            Type = type;
            _notificationCenter = notificationCenter;
            _container = task;
            WorkEffortAssociations = new List<IWorkEffortAssociation>();
            Comments = new List<string>();
            RequerdRole = roleType;

        }

        public WorkEffort(WorkEffortType type, dynamic task = null)
        {
            Id = Guid.NewGuid().ToString();
            Type = type;
            WorkEffortAssociations = new List<IWorkEffortAssociation>();
            Comments = new List<string>();
            _container = task;
        }
        #endregion

    }
}
