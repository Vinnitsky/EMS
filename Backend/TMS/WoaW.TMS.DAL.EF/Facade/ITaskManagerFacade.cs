using System.Collections.Generic;
using EzBpm.Tms.ConfigModel;
using WoaW.TMS.Model.DAL;

namespace WoaW.TMS.DAL.EF.Facade
{
    public interface ITaskManagerFacade
    {
        /// <summary>
        /// метод создает задачу
        /// </summary>
        /// <param name="taskTypeId"></param>
        /// <returns></returns>
        ITask Create(string taskTypeId);
        /// <summary>
        /// пользователь currentUserId назначает задачу taskId пользователю employeeId
        /// </summary>
        /// <remarks>
        /// если метод CanAssign возвращает false то выходим
        /// для того чтобы метод выполнился следующие условия должны быть истинными 
        /// - идентификатор задачи был не пустым
        /// - задача с указанным идентификатором существовала в базе
        /// - задача не была связана ни с какми сорудником 
        ///     - задача никем в данный момент не выполнялась
        ///     - задача никаому не была назначена
        ///     - задача не стояла ни у кого на паузе 
        /// - сотрудник был в группе исполнителей этого типа задач
        /// </remarks>
        /// <param name="currentUserId"></param>
        /// <param name="employeeId"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        ITask Assign(string employeeId, string taskId);
        /// <summary>
        /// возвращает список типов задач которые текущий пользователь может создавать
        /// </summary>
        /// <returns></returns>
        IEnumerable<TaskModel> WhichTasksUserCanCreate();
        /// <summary>
        /// пользователь принимает назаначенную задачу
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        ITask Accept(string taskId);
        /// <summary>
        /// пользователь отклоняет назначенную задачу
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        ITask Reject(string taskId);
        /// <summary>
        /// пользователь приступает к работе над задачей
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        ITask GetToWork(string taskId);
        /// <summary>
        /// пользователь приостанавливает работу над задачей - ставит ее на паузу
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        ITask Pause(string taskId);
        /// <summary>
        /// пользователь продолжает выполнение задачи, которая была поставленна на паузу
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        ITask Resume(string taskId);
        /// <summary>
        /// закрывает задачу.
        /// </summary>
        /// <param name="taskTypeId"></param>
        /// <returns></returns>
        ITask Close(string taskTypeId);
        /// <summary>
        /// прерывает выполнение задачи и переводит ее в состояние 'прервана'
        /// </summary>
        /// <returns></returns>
        ITask Cancel();

        /// <summary>
        /// изменяет текущего менеджера задачи taskId на того что пользователя что указан в managerId
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="managerId"></param>
        /// <returns></returns>
        ITask ChangeManager(string taskId, string managerId);
        QueryTaskResult Query(QueryTaskRequest request);
        ITask GetTaskById(string id);
    }
}
