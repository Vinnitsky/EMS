using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WoaW.CRM.Model.Persons
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// этот класс не может быть объявлен как sealed потому что 
    /// должен иметь virtual свойсвто для того чтобы EF смог 
    /// подгружать его по требованию... а как известно saled 
    /// класс не может иметь virtual методов... по этому класс 
    /// не запечатан
    /// </remarks>
    public class PersonName : INotifyPropertyChanged
    {
        #region attributes
        private string _name;
        private DateTime? _from;
        private DateTime? _thru;
        private PersonNameType _personNameType;
        private PersonNameOption _selectedOptions;
        #endregion

        #region properties
        public string Id { get; private set; }

        /// <summary>
        /// должен возвращать имя - кторое в SelectedOption
        /// должно былть полностью одинаково с _selectedOptions.Value
        /// но иногда может отличаться если в списке нет опций
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name)
                    return;

                _name = value;
                RaisePropertyChanged();
            }
        }
        public DateTime? FromDate
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
        public PersonNameType Type
        {
            get { return _personNameType; }
            set
            {
                if (value == _personNameType)
                    return;

                _personNameType = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// по сути это есть значение для конткретного типа 
        /// выбранное из возмможных вариантов для этого типа 
        /// </summary>
        virtual public PersonNameOption SelectedOption
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
        public PersonName()
        {
            Id = Guid.NewGuid().ToString();
        }
        public PersonName(DateTime? from = null, DateTime? thru = null)
            : this()
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
