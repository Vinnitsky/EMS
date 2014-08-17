using System.Collections.Generic;
using WoaW.CMS.DAL.EF;

namespace WoaW.TMS.Model.DAL
{
    public interface IActionAvailabilityValidator
    {
        /// <summary>
        /// Позволяет определить может ли пользователь создать задачу такого типа.
        /// </summary>
        /// <param name="userId">Текущий пользователь</param>
        /// <param name="taskTypeId">Идентификатор типа задачи. <see cref="TaskModel"/></param>
        /// <remarks>
        /// Создавать задачу могут только пользователи играющие роль менеджера.
        /// Менеджером задачи становиться пользователь который создал задачу, имзенить это могу только люди играющие роль супервайзера.
        /// 
        /// 1 - если идентификатор пользователя пустой - исключение
        /// 2 - если идентификатор типа задачи пустой - исключение
        /// </remarks>
        /// <returns></returns>
        bool CanCreate(string taskTypeId);

        /// <summary>
        /// проверять что задача находиться в состоянии Wait и текущий сотрудник входит в роль Manager‏
        /// true - свободен, false - занят 
        /// </summary>
        bool AllowAssign(string taskId);

        /// <summary>
        /// вернуть всех пользователей в системе которые входят в роль Employee для задачи
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="taskTypeIdparam>
        /// <returns></returns>
        IList<PartyIdentity> GetEmployees(string taskTypeId);
        /// <summary>
        /// метод проверяет возможность назначит задачу (taskId) сотруднику(userId) 
        /// проверять что задача находиться в состоянии Wait и текущий сотрудник входит в роль Manager, а toEmployeeId входит в роль Employee‏
        /// </summary>
        /// <remarks>
        /// 1 - если идентификатор пользоваеля пустой - исключение
        /// 2 - если идентификатор задачи пустой - исключение
        /// 3 - если пользователь (userId) уже выполняет какую-либо задачу - возвращаем false
        /// 5 - если задачу (taskId) уже кто-то выполняет - возвращаем false
        /// 6 - если пользователь не в группе менеджеров для типа задачи - возвращаем false
        /// 7 - если задача в состоянии wait то возвращаем true
        /// </remarks>
        /// <param name="userId">пользователь которому нужно назначить задачу</param>
        /// <param name="taskId">задача которую нужно назначить</param>
        /// <returns> true - если задача(taskId) может быть назначена 
        /// сотруднику (userId) и false в противном случае</returns>
        bool CanAssign(string employeeId, string taskId);
        /// <summary>
        /// проверяет если ли у пользователя право принять задачу
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        bool CanAccept(string taskId);
        /// <summary>
        /// проверят есть ли у пользователя право отклонить задачу
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        bool CanReject(string taskId);
        /// <summary>
        /// проверяет если возможность у пользователя выполнить задачу
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        bool CanGetToWork(string taskId);
        /// <summary>
        /// проверяет есть ли у пользователя право поставить задачу на паузу
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        bool CanPause(string taskId);
        /// <summary>
        /// проверяет, есть ли у пользователя право продолжить выполнение задачи
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        bool CanResume(string taskId);
        /// <summary>
        /// проверяет возможность закрыть задачу
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="taskTypeId"></param>
        /// <returns></returns>
        bool CanClose(string taskTypeId);
        /// <summary>
        /// используется менеджером для отмены любого дейсвтия сотрудника. 
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        bool CanCancel(string taskId);

        bool CanChangeDueDateTime(string taskId);
        bool CanChangeScheduleDateTime(string taskId);
        bool CanChangePriority(string taskId);

        /// <summary>
        /// Позволяет определить может ли пользователь сменить менеджера.
        /// </summary>
        /// <param name="userId">Текущий пользователь</param>
        /// <param name="taskId">Идентификатор задачи</param>
        /// <remarks>ТОлько супервайзер может менять менеджера</remarks>
        /// <returns></returns>
        bool CanChangeManager(string taskId);
        /// <summary>
        /// Позволяет определить может ли пользователь изменять данные задачи.
        /// </summary>
        /// <param name="userId">Текущий пользователь</param>
        /// <param name="taskId">Идентификатор задачи</param>
        /// <remarks>
        /// В задаче есть три основных роли.
        /// Супер вайзор не может редактировать данные задачи, он может только сменить менеджера.
        /// Менеджер может редактировать данные задачи только в случае если он еще не выполняеться и не закрыта.
        /// Исполнитель может редактировать данные задачи только в случае если он взял ее на выполнение и еще не закрыл.
        /// </remarks>
        /// <returns></returns>
        bool CanEdit(string taskId);

    }
}
