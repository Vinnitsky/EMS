using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WoaW.CMS.Model.Repationships
{
    public sealed class RelationshipType : INotifyPropertyChanged
    {
        #region attributes
        private string _title;
        private string _description;
        private ObservableCollection<RoleType> _roleTypes;
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
        public ICollection<RoleType> RoleTypes { get { return _roleTypes; } }
        #endregion

        #region constructors
        public RelationshipType()
        {
            Id = Guid.NewGuid().ToString();
            _roleTypes = new ObservableCollection<RoleType>();
        }
        public RelationshipType(string aTitle, RoleType[] roleTypes, string anId = null)
            : this()
        {
            Id = anId;
            Title = aTitle;

            foreach (var roleType in roleTypes)
            {
                _roleTypes.Add(roleType);
            }
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
