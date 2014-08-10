using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WoaW.CRM.Model.Identities;

namespace WoaW.CRM.Model.Persons
{
    public sealed class Citizenship : INotifyPropertyChanged
    {
        #region attributes
        private string _title;
        private Country _forCountry;
        private ObservableCollection<IdentityDocument> _identities;
        #endregion

        #region Properties
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
        public Country ForCountry
        {
            get { return _forCountry; }
            set
            {
                if (value == _forCountry)
                    return;

                _forCountry = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// TODO: возможно здесь нужно использовать базовый документ потому
        /// что например дети до 16 лет не имеют паспорта
        /// </summary>
        public ICollection<IdentityDocument> Identities { get { return _identities; } }
        #endregion

        #region constructors
        public Citizenship()
        {
            Id = System.Guid.NewGuid().ToString();
            _identities = new ObservableCollection<IdentityDocument>();
            _identities.CollectionChanged += _passports_CollectionChanged;
        }
        public Citizenship(IEnumerable<IdentityDocument> aIdentities, string title, string id = null)
            : this()
        {
            if (string.IsNullOrWhiteSpace(id) == false)
                Id = id;

            Title = title;

            foreach (var identity in aIdentities)
            {
                Identities.Add(identity);
            }
        }
        #endregion

        #region event handlers
        void _passports_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            throw new NotImplementedException();
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
