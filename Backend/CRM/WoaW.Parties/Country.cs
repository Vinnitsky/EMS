using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WoaW.CRM.Model
{
    public sealed class Country : INotifyPropertyChanged
    {
        #region attributes
        private string _title;
        private string _Iso2Name;
        private string _Iso3Name;
        #endregion

        #region properties
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
        public string Iso2Name
        {
            get { return _Iso2Name; }
            set
            {
                if (value == _Iso2Name)
                    return;

                _Iso2Name = value;
                RaisePropertyChanged();
            }
        }
        public string Iso3Name
        {
            get { return _Iso3Name; }
            set
            {
                if (value == _Iso3Name)
                    return;

                _Iso3Name = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region constructors
        public Country()
        {

        }
        public Country(string title, string iso2 = null, string iso3 = null)
        {
            Title = title;
            Iso2Name = iso2;
            Iso3Name = iso3;
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
