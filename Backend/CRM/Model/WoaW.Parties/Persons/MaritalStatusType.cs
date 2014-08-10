using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WoaW.CMS.Model.Persons
{
    /// <summary>
    /// 
    /// </summary>
    /// <see cref="http://www.statcan.gc.ca/concepts/definitions/marital-matrimonial04-eng.htm"/>
    /// <see cref="http://www.statcan.gc.ca/concepts/definitions/marital-matrimonial01-eng.htm"/>
    public sealed class MaritalStatusType : INotifyPropertyChanged
    {
        #region attributes
        private string _title;
        private string _description;
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
        #endregion

        #region constructors
        public MaritalStatusType()
        {
            Id = System.Guid.NewGuid().ToString();
        }
        public MaritalStatusType(string aTitle, string id = null)
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

        public static MaritalStatusType NoKnown = new MaritalStatusType("NoKnown", "0");
        public static MaritalStatusType Married = new MaritalStatusType("Married", "1");
        public static MaritalStatusType Widowed = new MaritalStatusType("Widowed", "2");
        public static MaritalStatusType Separated = new MaritalStatusType("Separated", "3");
        public static MaritalStatusType Divorced = new MaritalStatusType("Divorced", "4");
    }
}
