using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoaW.NS
{
    public interface INotification
    {
        DateTime OpenedTime { get; }
        DateTime ClosedTime { get; }
        string Description { get; set; }
        bool IsClosed { get; set; }
        ENotificationType Type { get; }
    }
}
