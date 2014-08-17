using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Data.Entity;//QueryableExtensions;
using System.Threading.Tasks;
using WoaW.CMS.Model;

namespace WoaW.CMS.DAL.EF
{
    public class SingleUserStore : UserStore<PartyIdentity>
    {
        public SingleUserStore(DbContext dbContext)
            : base((DbContext)dbContext)
        {
            DisposeContext = false;
        }

        public override Task CreateAsync(PartyIdentity identity)
        {
            var party = new Party(identity.UserName,identity.Id);
            //party.AppIdentity = user;
            //party.Identities.Add(identity);
            identity.Party = party;

            return base.CreateAsync(identity);
        }

        public override Task DeleteAsync(PartyIdentity identity)
        {
            var party = Context.Set<Party>().SingleOrDefault(p => p.Id == identity.Id);
            if (party != null)
                Context.Set<Party>().Remove(party);

            return base.DeleteAsync(identity);
        }

    }
}
