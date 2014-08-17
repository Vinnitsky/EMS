using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WoaW.CMS.Model.Identities;

namespace WoaW.CMS.Model
{

    public class Party: INotifyPropertyChanged
    {
        #region attributes
        private string _title;
        private string _description;
        private string _lockedByTaskId;
        private string _version;
        private System.DateTime _fromDate;
        private System.DateTime _thruDate;
        private Signature _signature;
        private ObservableCollection<Identification> _identifications;
        #endregion

        #region properties
        public string Id { get; set; }
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
        /// <summary>
        /// it is a task container Id
        /// if value null it means resource not locked
        /// </summary>
        public string LockedByTaskId
        {
            get { return _lockedByTaskId; }
            set
            {
                if (value == _lockedByTaskId)
                    return;

                _lockedByTaskId = value;
                RaisePropertyChanged();
            }
        }
        public System.DateTime FromDate
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
        public System.DateTime ThruDate
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
        public string Version
        {
            get { return _version; }
            set
            {
                if (value == _version)
                    return;

                _version = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// используется в TMS в частоности в TaskManagerFasade 
        /// </summary>
        public bool IsBusy { get; set; }

        virtual public Signature Signature
        {
            get { return _signature; }
            set
            {
                if (value == _signature)
                    return;

                _signature = value;
                RaisePropertyChanged();
            }
        }
        virtual public ObservableCollection<Identification> Identifications
        {
            get { return _identifications; }
            set
            {
                if (value == _identifications)
                    return;

                _identifications = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region constructors
        public Party()
        {
            Id = Guid.NewGuid().ToString();
            Identifications = new ObservableCollection<Identification>();
        }
        public Party(string aTitle, string id=null)
            : this()
        {
            #region parameter validation
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException("id");
            if (string.IsNullOrWhiteSpace(aTitle))
                throw new ArgumentNullException("aTitle");
            #endregion

            if (string.IsNullOrWhiteSpace(id) == false)
                Id = id;
            Title = aTitle;
        }
        #endregion

        #region INotifyPropertyChanged implementation
        protected void RaisePropertyChanged([CallerMemberName] string aPropertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(aPropertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

    }
}
