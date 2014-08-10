using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WoaW.CMS.Model.Persons
{
    public class PhysicalCharacteristic : INotifyPropertyChanged
    {
        #region attributes
        private PhysicalCharacteristicType _type;
        private DateTime _from;
        private DateTime? _thru;
        private string _value;
        private ObservableCollection<PhisicalCharacteristicOption> _selectedOptions;
        #endregion

        #region properties
        public string Id { get; private set; }
        public PhysicalCharacteristicType Type
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
        virtual public ObservableCollection<PhisicalCharacteristicOption> SelectedOptions
        {
            get { return _selectedOptions; }
            set
            {
                if (value == _selectedOptions)
                    return;

                _selectedOptions = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region constructors
        public PhysicalCharacteristic()
        {
            Id = System.Guid.NewGuid().ToString();
            _selectedOptions = new ObservableCollection<PhisicalCharacteristicOption>();
        }
        public PhysicalCharacteristic(DateTime? from = null, DateTime? thru = null)
            :this()
        {
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
