using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WoaW.CMS.Model.Repationships
{
    public class PartyRelationship : INotifyPropertyChanged
    {
        #region attributes
        private DateTime _fromDate;
        private DateTime? _thruDate;
        private RelationshipType _type;
        private ObservableCollection<PartyRole> _roles;
        private string _comment;
        #endregion

        #region properties
        public string Id { get; set; }
        public string Comment
        {
            get { return _comment; }
            set
            {
                if (value == _comment)
                    return;

                _comment = value;
                RaisePropertyChanged();
            }
        }

        public DateTime FromDate
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
        public DateTime? ThruDate
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
        public RelationshipType Type
        {
            get { return _type; }
            set
            {
                if (value == _type)
                    return;

                _type = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<PartyRole> Roles { get { return _roles; } }
        #endregion

        #region constructors
        public PartyRelationship()
        {
            Id = Guid.NewGuid().ToString();
            _roles = new ObservableCollection<PartyRole>();
            _roles.CollectionChanged += _roles_CollectionChanged;
        }
        public PartyRelationship(string anId, RelationshipType aType, DateTime? aFrom = null, DateTime? aThru = null)
            : this()
        {
            Id = anId;
            _type = aType;
            _fromDate = aFrom ?? DateTime.Now;
            _thruDate = aThru;
        }
        public PartyRelationship(RelationshipType relationshipType, PartyRole[] partyRoles)
            : this()
        {
            Type = relationshipType;
            foreach (var role in partyRoles)
            {
                AddRole(role);
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
        
        void _roles_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    foreach (PartyRole role in e.NewItems)
                    {
                        if (!IsRoleValid(role.Type))
                        {
                            _roles.Remove(role);
                            throw new ArgumentException(string.Format("relationship '{0}' does not containe role '{1}'",
                                Type.Title, role.Type));
                        }
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
        }
        private bool IsRoleValid(RoleType aRoleType)
        {
            if (aRoleType == null)
                throw new ArgumentNullException("aRoleType");

            return Type.RoleTypes.Contains(aRoleType);
        }
        public void AddRole(PartyRole role)
        {
            if (!IsRoleValid(role.Type))
                throw new ArgumentException(string.Format("relationship '{0}' does not containe role '{1}'",
                    Type.Title, role.Type.Title));
            else
                _roles.Add(role);
        }
    }
}
