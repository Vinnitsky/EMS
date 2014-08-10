using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WoaW.CRM.Model.Repationships
{
    public sealed class RoleType : INotifyPropertyChanged
    {
        #region attributes
        private string _title;
        private string _description;
        #endregion

        #region properties
        public string Id { get; private set; }
        public string Title
        {
            get { return _title; }
            set
            {
                if (value == _title)
                    return;

                _title = value;
                RaisePropertyChanged();
            }
        }
        public string Description
        {
            get { return _description; }
            set
            {
                if (value == _description)
                    return;

                _description = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region constructors
        public RoleType()
        {
            Id = System.Guid.NewGuid().ToString();
        }
        public RoleType(string aTitle, string anId = null)
            : this()
        {
            Id = anId;
            Title = aTitle;
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
