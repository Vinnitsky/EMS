using System.Collections.Generic;

namespace WoaW.CMS.Model.Clasifications
{
    public class PartyClasification
    {
        public string Id { get; private set; }
        public System.DateTime FromDate { get; set; }
        public System.DateTime? ThruDate { get; set; }
        virtual public PartyType PartyType { get; set; }
        virtual public IEnumerable<Party> Party { get; set; }

        public PartyClasification()
        {
            Id = System.Guid.NewGuid().ToString();
            FromDate = System.DateTime.Now;
        }
    }
}
