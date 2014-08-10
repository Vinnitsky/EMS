using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoaW.NS
{
    public interface INotificationCenter
    {
        ObservableCollection<INotification> Notifications { get; }
    }
}
