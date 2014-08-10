using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WoaW.CMS.Model.Repationships
{
    public class PartyRole : INotifyPropertyChanged
    {
        #region attributes
        private Party _party;
        private RoleType _type;
        private DateTime _fromDate;
        private DateTime? _thruDate;

        #endregion

        #region propeties
        public string Id { get; private set; }
        public Party Party
        {
            get { return _party; }
            set
            {
                if (value == _party)
                    return;

                _party = value;
                RaisePropertyChanged();
            }
        }
        public RoleType Type
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
        public PartyRole()
        {
            Id = Guid.NewGuid().ToString();
        }
        public PartyRole(RoleType roleType, Party party)
            : this()
        {
            #region parameter validation
            if (roleType == null)
                throw new ArgumentNullException("roleType");
            if (party == null)
                throw new ArgumentNullException("party");
            #endregion

            Type = roleType;
            Party = party;
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
