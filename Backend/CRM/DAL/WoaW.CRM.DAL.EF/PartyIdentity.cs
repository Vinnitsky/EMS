using Microsoft.AspNet.Identity.EntityFramework;
using WoaW.CMS.Model;

namespace WoaW.CMS.DAL.EF
{
    public class PartyIdentity : IdentityUser
    {
        public PartyIdentity()
        {

        }
        public PartyIdentity(string userName)
            : base(userName)
        {

        }
        public PartyIdentity(string userName, string id)
            : base(userName)
        {
            Id = id;
        }


        virtual public Party Party { get; set; }

        public string Fio { get; set; }

        public bool IsDisabled { get; set; }

        public bool IsOnline { get; set; }
    }
}
