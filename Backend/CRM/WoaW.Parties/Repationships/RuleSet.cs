using System.Collections.ObjectModel;
namespace WoaW.CRM.Model.Repationships
{
    public class RuleSet
    {
        public string Id { get; private set; }
        public string AtributeTitle { get; set; }
        public string AtributeValue { get; set; }
        public string Description { get; set; }
        public System.DateTime FromDate { get; set; }
        public System.DateTime ThruDate { get; set; }
        public string Title { get; set; }
        virtual public RoleType Role { get; set; }
        virtual public RelationshipType Relationship { get; set; }
        virtual public ObservableCollection<RelationshipConstraine> Constraines { get; private set; }
        public RuleSet()
        {
        }

    }
}
