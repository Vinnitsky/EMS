using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoaW.NS
{
    public class Notification : INotification
    {
        #region attributes
        private DateTime _openedTime;
        private DateTime _closedTime;
        private string _description;
        private bool _isClosed;
        private ENotificationType _type;
       #endregion

        #region Properties
        public Guid Id { get; private set; }
        public DateTime OpenedTime { get { return _openedTime; } }
        public DateTime ClosedTime { get { return _closedTime; } }
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        public bool IsClosed
        {
            get { return _isClosed; }
            set { _isClosed = value; _closedTime = DateTime.Now; }
        }
        public ENotificationType Type { get { return _type; } }
        #endregion

        public Notification(ENotificationType type)
        {
            Id = Guid.NewGuid();
            _openedTime = DateTime.Now;
            _type = type;
        }


    }
}
