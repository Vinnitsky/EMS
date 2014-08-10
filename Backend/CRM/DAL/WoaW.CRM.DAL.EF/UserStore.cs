using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;//QueryableExtensions;
using System.Text;
using System.Threading.Tasks;
using WoaW.CRM.Model;

namespace WoaW.CRM.DAL.EF
{
    public class UserStore : IUserStore<User, string>, IUserPasswordStore<User, string>, IDisposable
    {
        private UserDbContext _identityContext;
        private CrmDbContext _crmContext;
        public UserStore()
        {
            _identityContext = new UserDbContext();
            _crmContext = new CrmDbContext();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #region IUserStore<T,K> members
        public Task CreateAsync(User user)
        {
            _identityContext.Set<User>().Add(user);
            _identityContext.SaveChangesAsync();

            var party = new Party(user.Title, user.Id);
            _crmContext.Set<Party>().Add(party);
            _crmContext.SaveChangesAsync();

            return Task.FromResult(true);
        }

        public Task UpdateAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(User user)
        {
            _identityContext.Set<User>().Remove(user);
            _identityContext.SaveChangesAsync();

            var party = new Party(user.Title, user.Id);
            _crmContext.Set<Party>().Add(party);
            _crmContext.SaveChangesAsync();

            return Task.FromResult(true);
        }

        public Task<User> FindByIdAsync(string userId)
        {
            return _identityContext.Set<User>().FirstOrDefaultAsync(user => user.Id == userId);
        }

        public Task<User> FindByNameAsync(string userName)
        {
            var user = _identityContext.Set<User>().SingleOrDefaultAsync(u => u.UserName.Equals(userName));
            return user;
        }
        #endregion

        #region IUserPasswordStore<User,int>
        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(true);
        }

        public Task<string> GetPasswordHashAsync(User user)
        {
            //return new Task<string>(() => user.PasswordHash);
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            throw new NotImplementedException();
        }
        #endregion


        void IDisposable.Dispose()
        {
            _identityContext.Dispose();
            _crmContext.Dispose();
        }
    }
}
