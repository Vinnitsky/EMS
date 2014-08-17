using System;
using System.Collections.Generic;
using WoaW.CMS.Model.Repationships;
using WoaW.NS;

namespace WoaW.TMS.Model
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
        #endregion

        #region properties
        /// <summary>
        /// идентификатор экземпляра
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// позволяет или запрещает руками назначать эту задачу.
        /// </summary>
        public bool AllowManualAcceptance { get; set; }
        /// <summary>
        /// время когда задача была создана
        /// </summary>
        public DateTime CreationTime { get; set; }
        /// <summary>
        /// предполагаемое время исполнения задачи
        /// </summary>
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
        public DateTime ScheduledStartTime { get; set; }
        /// <summary>
        /// время до которого задача должна быть выполена 
        /// </summary>
        public DateTime? DueDateTime { get; set; }
        /// <summary>
        /// поле которое указывает к какому скроку должна быть выполнена 
        /// </summary>
        public DateTime? DueFinishTime { get; set; }
        /// <summary>
        /// время когда сотрудник начал выполнять задачу
        /// в данной реализации считаем, что время принятия задачи 
        /// является актуальным временем начала задачи. 
        /// т.е. если принял - то начал выполнять
        /// </summary>
        public DateTime? ActualStartTime { get; set; }
        /// <summary>
        /// дата когда задача действительно была закрыта 
        /// Status.Csosed
        /// </summary>
        public DateTime? ActualFinishTime { get; set; }
        /// <summary>
        /// может работать с непосредственно таким же свойством задачи
        /// </summary>
        public int Priority { get; set; }
        /// <summary>
        /// может работать с непосредственно таким же свойством задачи
        /// </summary>
        virtual public RoleType RequerdRole { get; set; }
        /// <summary>
        /// в данной реализации список сожержит ссылки на ассоциированные 
        /// с данной задачи. в будущем можно создать класс WorkEffortAssociation
        /// который будет позволять ассоциировать задачи как по типу так и по ID 
        /// 
        /// сейчас это просто тип ассоциированной задачи
        /// </summary>
        virtual public List<WorkEffortAssociation> WorkEffortAssociations { get; set; }
        virtual public WorkEffortType Type { get; set; }
        public List<string> Comments { get; set; }
        #endregion

        #region constructors
        /// <summary>
        /// этот конструтуор нужен только для EF
        /// поскольку конструктор вызывается всякий раз как EF подымает объект с базы, 
        /// то в конструкторе нельзя иннициализировать переменнные времени жизни,
        /// например             CreationTime = DateTime.Now
        /// </summary>
        public WorkEffort()
        {
            Id = Guid.NewGuid().ToString();
            WorkEffortAssociations = new List<WorkEffortAssociation>();
            Comments = new List<string>();
        }
        public WorkEffort(WorkEffortType type)
            : this()
        {
            Type = type;
        }
        public WorkEffort(WorkEffortType type, INotificationCenter notificationCenter, RoleType requeredRole = null)
            : this()
        {
            Type = type;
            _notificationCenter = notificationCenter;
            RequerdRole = requeredRole;

        }

        #endregion

    }
}
