using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoaW.NS
{
    public class NotificationCenter : INotificationCenter
    {
        #region attributes
        private System.Collections.ObjectModel.ObservableCollection<INotification> _allerts;
        #endregion

        #region Properties
        public System.Collections.ObjectModel.ObservableCollection<INotification> Notifications { get { return _allerts; } }
        #endregion

        #region construnctors
        public NotificationCenter()
        {
            _allerts = new System.Collections.ObjectModel.ObservableCollection<INotification>();
            _allerts.CollectionChanged += _allerts_CollectionChanged;
        }

        void _allerts_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    OnAdd(e.NewItems);
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    OnAdd(e.NewItems);
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    OnAdd(e.OldItems);
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
        }
        #endregion

        protected virtual void OnAdd(IList items) { }
        protected virtual void OnMove(IList items) { }
        protected virtual void OnRemove(IList items) { }
        protected virtual void OnReplace(IList items) { }
        protected virtual void OnReset(IList items) { }
    }
}
