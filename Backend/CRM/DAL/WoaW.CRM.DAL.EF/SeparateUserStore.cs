using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;//QueryableExtensions;
using System.Text;
using System.Threading.Tasks;
using WoaW.CMS.Model;

namespace WoaW.CMS.DAL.EF
{
    internal class SeparateUserStore : UserStore<PartyIdentity>
    {
        public SeparateUserStore()
        {
        }


        override public Task CreateAsync(PartyIdentity user)
        {
            //_identityContext.Set<User>().Add(user);
            //_identityContext.SaveChangesAsync();

            //var party = new Party(user.UserName, user.Id);
            //_crmContext.Set<Party>().Add(party);
            //_crmContext.SaveChangesAsync();

            return Task.FromResult(true);
        }

        override public Task DeleteAsync(PartyIdentity user)
        {
            //_identityContext.Set<User>().Remove(user);
            //_identityContext.SaveChangesAsync();

            //var party = new Party(user.UserName, user.Id);
            //_crmContext.Set<Party>().Add(party);
            //_crmContext.SaveChangesAsync();

            return Task.FromResult(true);
        }
    }
}
