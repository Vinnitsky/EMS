using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;


namespace WoaW.TMS.Tasks
{
    public class Task1 : INotifyPropertyChanged
    {
        #region attributes
        private string _id;
        #endregion

        #region properties
        public string Id
        {
            get { return _id; }
            set
            {
                if (value == _id)
                    return;

                _id = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// var tokenSource2 = new CancellationTokenSource();
        /// CancellationToken ct = tokenSource2.Token;
        /// 
        /// токен для отмены задачи 
        /// </summary>
        public CancellationToken CancellationToken { get; set; }
        #endregion

        #region INotifyPropertyChanged implementation
        protected void RaisePropertyChanged([CallerMemberName] string aPropertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(aPropertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
    public class Task2 : INotifyPropertyChanged
    {
        #region attributes
        private string _id;
        #endregion

        #region properties
        public string Id
        {
            get { return _id; }
            set
            {
                if (value == _id)
                    return;

                _id = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// var tokenSource2 = new CancellationTokenSource();
        /// CancellationToken ct = tokenSource2.Token;
        /// 
        /// токен для отмены задачи 
        /// </summary>
        public CancellationToken CancellationToken { get; set; }
        #endregion

        #region INotifyPropertyChanged implementation
        protected void RaisePropertyChanged([CallerMemberName] string aPropertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(aPropertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
    public class Task3 : INotifyPropertyChanged
    {
        #region attributes
        private string _id;
        #endregion

        #region properties
        public string Id
        {
            get { return _id; }
            set
            {
                if (value == _id)
                    return;

                _id = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// var tokenSource2 = new CancellationTokenSource();
        /// CancellationToken ct = tokenSource2.Token;
        /// 
        /// токен для отмены задачи 
        /// </summary>
        public CancellationToken CancellationToken { get; set; }
        #endregion

        #region INotifyPropertyChanged implementation
        protected void RaisePropertyChanged([CallerMemberName] string aPropertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(aPropertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
