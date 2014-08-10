using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WoaW.CRM.Model.Persons
{
    /// <summary>
    /// ISO 5218
    /// 
    /// </summary
    public sealed class GenderType : INotifyPropertyChanged
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
        public GenderType()
        {
            Id = System.Guid.NewGuid().ToString();
        }
        public GenderType(string aTitle, string id = null)
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

        public static GenderType NoKnown = new GenderType("NoKnown", "0");
        public static GenderType Male = new GenderType("Male", "1");
        public static GenderType Female = new GenderType("Female", "2");
        public static GenderType NotApplicable = new GenderType("NotApplicable", "3");
    }
}
