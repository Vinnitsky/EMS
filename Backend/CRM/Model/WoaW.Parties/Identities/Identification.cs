using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WoaW.CMS.Model.Identities
{
    public class Identification : INotifyPropertyChanged
    {
        #region attributes
        private string _title;
        private string _description;
        private System.DateTime _validFrom;
        private System.DateTime _validTo;
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
        public System.DateTime ValidFrom
        {
            get { return _validFrom; }
            set
            {
                if (value == _validFrom)
                    return;

                _validFrom = value;
                RaisePropertyChanged();
            }
        }
        public System.DateTime ValidTo
        {
            get { return _validTo; }
            set
            {
                if (value == _validTo)
                    return;

                _validTo = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region constructors
        public Identification()
        {
            Id = System.Guid.NewGuid().ToString();
        }
        public Identification(string title, string id=null)
            : this()
        {
            if (string.IsNullOrWhiteSpace(id) == false)
                Id = id;

            Title = title;
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