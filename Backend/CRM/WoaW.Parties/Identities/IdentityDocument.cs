using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WoaW.CRM.Model.Identities
{
    public class IdentityDocument : INotifyPropertyChanged
    {
        #region attributes
        private string _num;
        private string _title;
        private string _authority;
        private DateTime _issueDate;
        private DateTime _expirationDate;
        private Country _forCountry;
        private ObservableCollection<PageScan> _scanPages;
        #endregion

        #region properties
        public string Id { get; private set; }
        public string Num
        {
            get { return _num; }
            set
            {
                if (value == _num)
                    return;

                _num = value;
                RaisePropertyChanged();
            }
        }
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
        public DateTime IssueDate
        {
            get { return _issueDate; }
            set
            {
                if (value == _issueDate)
                    return;

                _issueDate = value;
                RaisePropertyChanged();
            }
        }
        public DateTime ExpirationDate
        {
            get { return _expirationDate; }
            set
            {
                if (value == _expirationDate)
                    return;

                _expirationDate = value;
                RaisePropertyChanged();
            }
        }
        public string Authority
        {
            get { return _authority; }
            set
            {
                if (value == _authority)
                    return;

                _authority = value;
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
        virtual public ObservableCollection<PageScan> ScanPages
        {
            get { return _scanPages; }
            set
            {
                if (value == _scanPages)
                    return;

                _scanPages = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region constructors
        public IdentityDocument()
        {
            Id = System.Guid.NewGuid().ToString();
            _scanPages = new ObservableCollection<PageScan>();
        }
        public IdentityDocument(string aNum, string title, string anAuthority, DateTime anIssueDate, DateTime anExpirationDate)
            : this()
        {
            Id = aNum;
            Num = aNum;
            Title = title;
            Authority = anAuthority;
            IssueDate = anIssueDate;
            ExpirationDate = anExpirationDate;
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
