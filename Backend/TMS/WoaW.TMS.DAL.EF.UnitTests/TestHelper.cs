using System;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WoaW.CMS.DAL.EF;

namespace EzBpm.Tms.DAL.EF.UnitTests
{
    static class TestHelper
    {
        public static PartyIdentity RegisterUser(DbContext dbContext, TestContext testContext, string userName, string roleName)
        {
            //AuthenticationManager.SignOut();

            var roleManager = new RoleManager<IdentityRole, string>(new RoleStore<IdentityRole>(dbContext));
            var role = roleManager.FindByName(roleName);

            if (role == null)
            {
                //создаю роль 
                role = new IdentityRole(roleName);
                var result = roleManager.Create(role);
                if (!result.Succeeded)
                    throw new ApplicationException(string.Format("Can't create role={0}", role.Name));
            }
            testContext.Properties[roleName] = role.Name;

            //создаю пользователя 
            var userManager = new UserManager<PartyIdentity, string>(new SingleUserStore(dbContext));
            var employee = userManager.FindByName(userName);
            if (employee == null)
            {
                //создаем пользователя
                employee = new PartyIdentity(userName) { Email = "manager@server" };

                var userCreationResult1 = userManager.Create(employee, "root123");
                if (userCreationResult1.Succeeded == false)
                    throw new Exception(userCreationResult1.Errors.ToString());

                //добавляетм пользователя в группу  
                userManager.AddToRole(employee.Id, role.Name);
            }
            testContext.Properties[userName] = employee.Id;
            return employee;
        }
    }
}
