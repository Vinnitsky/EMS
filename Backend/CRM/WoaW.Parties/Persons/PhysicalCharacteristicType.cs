﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WoaW.CRM.Model.Persons
{
    public class PhysicalCharacteristicType : INotifyPropertyChanged
    {
        #region attributes
        private string _title;
        private string _description;
        private ObservableCollection<PhisicalCharacteristicOption> _possibleOptions;

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
        virtual public ObservableCollection<PhisicalCharacteristicOption> PossibleOptions
        {
            get { return _possibleOptions; }
            set
            {
                if (value == _possibleOptions)
                    return;

                _possibleOptions = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region constructors
        public PhysicalCharacteristicType()
        {
            Id = System.Guid.NewGuid().ToString();
            _possibleOptions = new ObservableCollection<PhisicalCharacteristicOption>();
        }
        public PhysicalCharacteristicType(string aTitle, string id = null)
            : this()
        {
            if (string.IsNullOrWhiteSpace(id) == false)
                Id = id;
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
