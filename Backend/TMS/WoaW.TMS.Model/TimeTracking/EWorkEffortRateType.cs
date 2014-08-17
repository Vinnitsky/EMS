using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WoaW.TMS.Model.TimeTracking
{
    public enum EWorkEffortRateType
    {
        /// <summary>
        /// стоимость позиции
        /// </summary>
        PositionRate, 
        /// <summary>
        /// стоимость конкретного человека (контрактора или сотрудника)
        /// </summary>
        PartyRate, 
        /// <summary>
        /// стоимость задачи, активности
        /// </summary>
        WorkEffortRate

    }
}
