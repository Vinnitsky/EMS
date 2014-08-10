
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WoaW.CMS.Model.Persons
{
    public sealed class Gender : INotifyPropertyChanged
    {
        #region attributes
        private GenderType _type;
        private DateTime _from;
        private DateTime? _thru;
        #endregion

        #region properties
        public string Id { get; private set; }
        public GenderType Type
        {
            get { return _type; }
            set
            {
                if (value == _type)
                    return;

                _type = value;
                RaisePropertyChanged();
            }
        }
        public DateTime FromDate
        {
            get { return _from; }
            set
            {
                if (value == _from)
                    return;

                _from = value;
                RaisePropertyChanged();
            }
        }
        public DateTime? ThruDate
        {
            get { return _thru; }
            set
            {
                if (value == _thru)
                    return;

                _thru = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region constructors
        public Gender()
        {
            Id = Guid.NewGuid().ToString();
        }
        public Gender(GenderType type, string id = null, DateTime? from = null, DateTime? thru = null)
            : this()
        {
            if (string.IsNullOrWhiteSpace(id) == false)
                Id = id;

            Type = type;
            FromDate = from ?? DateTime.Now;
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
