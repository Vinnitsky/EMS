using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WoaW.CRM.Model.Persons
{
    public class Person : Party
    {
        #region attributes
        private string _SSID;
        private DateTime? _birthdate;
        private ObservableCollection<Gender> _genders;
        private ObservableCollection<Citizenship> _citizenships;
        private ObservableCollection<MaritalStatus> _meritalStatuses;
        private ObservableCollection<PhysicalCharacteristic> _phisicalCHaracteristics;
        private ObservableCollection<PersonName> _names;
        #endregion

        #region Properties
        public string SSID
        {
            get { return _SSID; }
            set
            {
                if (value == _SSID)
                    return;

                _SSID = value;
                RaisePropertyChanged();
            }
        }
        public DateTime? Birthdate
        {
            get { return _birthdate; }
            set
            {
                if (value == _birthdate)
                    return;

                _birthdate = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<Gender> Genders { get { return _genders; } }
        public GenderType Gender { get { return (from g in _genders orderby g.FromDate select g.Type).LastOrDefault(); } }
        public ObservableCollection<Citizenship> Citizenships { get { return _citizenships; } }
        public ObservableCollection<MaritalStatus> MeritalStatuses { get { return _meritalStatuses; } }
        public ObservableCollection<PhysicalCharacteristic> PhisicalCHaracteristics { get { return _phisicalCHaracteristics; } }
        public ObservableCollection<PersonName> PersonalNames { get { return _names; } }

        #endregion

        #region constructors
        public Person()
        {
            _genders = new ObservableCollection<Gender>();

            _citizenships = new ObservableCollection<Citizenship>();
            _citizenships.CollectionChanged += _citizenships_CollectionChanged;
            _meritalStatuses = new ObservableCollection<MaritalStatus>();
            _meritalStatuses.CollectionChanged += _meritalStatuses_CollectionChanged;
            _phisicalCHaracteristics = new ObservableCollection<PhysicalCharacteristic>();
            _phisicalCHaracteristics.CollectionChanged += _phisicalCHaracteristics_CollectionChanged;
            _names = new ObservableCollection<PersonName>();
            _names.CollectionChanged += _names_CollectionChanged;
        }

        public Person(string aTitle, string anId = null, GenderType aGender = null, MaritalStatusType aMaritalStatus = null)
            : base(aTitle, anId)
        {
            _genders = new ObservableCollection<Gender>();

            _citizenships = new ObservableCollection<Citizenship>();
            _citizenships.CollectionChanged += _citizenships_CollectionChanged;
            _meritalStatuses = new ObservableCollection<MaritalStatus>();
            _meritalStatuses.CollectionChanged += _meritalStatuses_CollectionChanged;
            _phisicalCHaracteristics = new ObservableCollection<PhysicalCharacteristic>();
            _phisicalCHaracteristics.CollectionChanged += _phisicalCHaracteristics_CollectionChanged;
            _names = new ObservableCollection<PersonName>();
            _names.CollectionChanged += _names_CollectionChanged;

            _genders.Add(new Gender(aGender ?? GenderType.NoKnown));
            _meritalStatuses.Add(new MaritalStatus(aMaritalStatus ?? MaritalStatusType.NoKnown));
        }

        #endregion

        #region event handlers
        void _citizenships_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
        }
        void _meritalStatuses_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
        }
        void _phisicalCHaracteristics_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
        }
        void _names_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
        }
        #endregion

    }
}
