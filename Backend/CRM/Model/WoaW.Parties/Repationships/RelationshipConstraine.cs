using System.Collections.Generic;

namespace WoaW.CMS.Model.Repationships
{
    public class RelationshipConstraine
    {
        public string Description { get; set; }
        public System.DateTime FromDate { get; set; }
        public string Id { get; private set; }
        public System.DateTime ThruDate { get; set; }
        public string Title { get; set; }
        virtual public IEnumerable<PartyRelationship> Relationships { get; set; }
        virtual public RoleType Role { get; set; }

        public RelationshipConstraine()
        {
        }

    }
}