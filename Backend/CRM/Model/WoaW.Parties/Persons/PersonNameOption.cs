using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WoaW.CMS.Model.Persons
{
    public class PersonNameOption : INotifyPropertyChanged
    {
        #region attributes
        private string _value;
        private PersonNameType _forPersonNameType;
        #endregion

        #region properties
        public string Id { get; private set; }
        public string Value
        {
            get { return _value; }
            set
            {
                if (value == _value)
                    return;

                _value = value;
                RaisePropertyChanged();
            }
        }
        virtual public PersonNameType ForPersonNameType
        {
            get { return _forPersonNameType; }
            set
            {
                if (value == _forPersonNameType)
                    return;

                _forPersonNameType = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region constrictors
        public PersonNameOption()
        {
            Id = System.Guid.NewGuid().ToString();
        }
        public PersonNameOption(string value, PersonNameType type)
            : this()
        {
            _value = value;
            _forPersonNameType = type;
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