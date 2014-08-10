using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WoaW.CMS.Model.Persons
{
    public class PhisicalCharacteristicOption : INotifyPropertyChanged
    {
        #region attributes
        private string _value;
        private PhysicalCharacteristicType _forPhysicalCharacteristicType;
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
        virtual public PhysicalCharacteristicType ForPhysicalCharacteristicType
        {
            get { return _forPhysicalCharacteristicType; }
            set
            {
                if (value == _forPhysicalCharacteristicType)
                    return;

                _forPhysicalCharacteristicType = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region constrictors
        public PhisicalCharacteristicOption()
        {
            Id = System.Guid.NewGuid().ToString();
        }
        public PhisicalCharacteristicOption(string value, PhysicalCharacteristicType type)
            : this()
        {
            _value = value;
            _forPhysicalCharacteristicType = type;
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