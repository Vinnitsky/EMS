using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WoaW.CRM.Model.Persons
{
    public sealed class MaritalStatus : INotifyPropertyChanged
    {
        #region attributes
        private MaritalStatusType _meritalStatusType;
        private DateTime? _fromDate;
        private DateTime? _thruDate;
        #endregion

        #region properties
        public string Id { get; private set; }
        /// <summary>
        /// described by type 
        /// </summary>
        public MaritalStatusType Type
        {
            get { return _meritalStatusType; }
            set
            {
                if (value == _meritalStatusType)
                    return;

                _meritalStatusType = value;
                RaisePropertyChanged();
            }
        }
        public DateTime? FromDate
        {
            get { return _fromDate; }
            set
            {
                if (value == _fromDate)
                    return;

                _fromDate = value;
                RaisePropertyChanged();
            }
        }
        public DateTime? ThruDate
        {
            get { return _thruDate; }
            set
            {
                if (value == _thruDate)
                    return;

                _thruDate = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region constructors
        public MaritalStatus()
        {
            Id = Guid.NewGuid().ToString();
        }
        public MaritalStatus(MaritalStatusType type, DateTime? from=null, DateTime? thru=null)
            : this()
        {
            Type = type;
            FromDate = from??DateTime.Now;
            ThruDate = thru;
        }
        #endregion

        #region INotifyPropertyChanged implementation
        private void RaisePropertyChanged([CallerMemberName] string aPropertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(aPropertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

    }
}
