using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;

namespace WoaW.CMS.DAL.EF
{
    public static class UserManagerHelper
    {
        static public IList<PartyIdentity> GetUsersForRoleName(this UserManager<PartyIdentity, string> userManager, RoleManager<IdentityRole, string> roleManager, string roleName)
        {
            #region parameter validation

            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentNullException("roleName");

            if(roleManager == null)
                throw new ArgumentNullException("roleManager");
            #endregion

            var role = roleManager.FindByName(roleName);
            if(role == null)
                throw new ArgumentException("can find role with name {0}", roleName);
            var users = userManager.Users.ToList();
            var userInRoles = users.Where(user => userManager.IsInRole(user.Id, role.Name));
            return userInRoles.ToList();
        }
        static public PartyIdentity[] GetUsersForRoleId(this UserManager<PartyIdentity, string> userManager, RoleManager<IdentityRole, string> roleManager, string roleId)
        {
            #region parameter validation

            if (string.IsNullOrWhiteSpace(roleId))
                throw new ArgumentNullException("roleId");

            if (roleManager == null)
                throw new ArgumentNullException("roleManager");
            #endregion

            var role = roleManager.FindById(roleId);
            var users = userManager.Users.ToList();
            var userInRoles = users.Where(user => userManager.IsInRole(user.Id, role.Name));
            return userInRoles.ToArray();
        }

        static public IdentityRole[] GetRolesForUser(this UserManager<PartyIdentity, string> userManager, RoleManager<IdentityRole, string> roleManager, string userId)
        {
            #region parameter validation

            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException("userId");

            if (roleManager == null)
                throw new ArgumentNullException("roleManager");
            #endregion

            var roles = roleManager.Roles;
            return userManager.GetRoles(userId).Select(x => roles.First(r => r.Name == x)).ToArray();
        }


        static public void CheckUsersOffline(this UserManager<PartyIdentity, string> userManager)
        {
            //TODO: finish
            //var dt = SystemTime.Now - TimeSpan.FromMinutes(TimeToOffline);
            //var users = _dbContext.Set<ApplicationUser>().Include(x => x.CurrentWorkDuration)
            //    .Where(x => (x.LastOnlineCallbackTimestamp == null || x.LastOnlineCallbackTimestamp < dt) && x.IsOnline)
            //    .ToList();
            //foreach (var applicationUser in users)
            //{
            //    if (applicationUser.CurrentWorkDuration != null)
            //    {
            //        applicationUser.CurrentWorkDuration.End = SystemTime.Now;
            //        applicationUser.CurrentWorkDuration = null;
            //    }
            //    applicationUser.IsOnline = false;
            //}
            //_dbContext.FlushChanges();
        }

        static public void UserOffline(this UserManager<PartyIdentity, string> userManager, string userName)
        {
            //TODO: finish - это оказывается аналог логина. 

            //var user = userManager.FindByName(userName);
            //if (user.IsOnline == true)
            //{
            //    user.IsOnline = false;
            //    if (user.CurrentWorkDuration != null)
            //    {
            //        user.CurrentWorkDuration.End = SystemTime.Now;
            //    }
            //    _dbContext.FlushChanges();
            //}
        }

        static public void UserOnline(this UserManager<PartyIdentity, string> userManager, string userName)
        {
            //TODO: finish

            //if (userName == null) throw new ArgumentNullException("userName");

            //var user = _userManager.FindByName(userName);
            //if (user.IsOnline == false)
            //{
            //    user.CurrentWorkDuration = new UserWorkDuration
            //    {
            //        Id = Guid.NewGuid().ToString(),
            //        Start = SystemTime.Now,
            //        User = user
            //    };
            //    _dbContext.Add(user.CurrentWorkDuration);
            //}
            //user.IsOnline = true;
            //user.LastOnlineCallbackTimestamp = SystemTime.Now;
            //_dbContext.FlushChanges();
        }

    }
}
