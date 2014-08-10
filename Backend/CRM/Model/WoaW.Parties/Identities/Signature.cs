using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WoaW.CMS.Model.Identities
{
    public class Signature : INotifyPropertyChanged
    {
        #region attributes
        private string _description;
        private byte[] _image;
        private System.DateTime _validFrom;
        private System.DateTime _validTo;
        #endregion

        #region properties
        public string Id { get; private set; }
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
        public byte[] Image
        {
            get { return _image; }
            set
            {
                if (value == _image)
                    return;

                _image = value;
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
        public Signature()
        {
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