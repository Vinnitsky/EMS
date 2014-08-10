using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.Data.Entity;
using WoaW.CRM.Model.Persons;
using System.Data.Entity.Validation;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Infrastructure;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net;

namespace WoaW.CRM.DAL.EF.UnitTests
{
    /// <summary>
    /// Summary description for UnitTest2
    /// </summary>
    [TestClass]
    public class UnitTest2
    {
        IDictionary<string, Action> initActions = new Dictionary<string, Action>();

        public UnitTest2()
        {
            initActions["CreatePerson_SuccessTest"] = CreatePerson_SuccessTest_Init;
            initActions["CreateUser_UsingDBContext_FailTest"] = CreateUser_UsingDBContext_FailTest_Init;
            initActions["CreateUser_UsingUserManager_FailTest"] = CreateUser_UsingUserManager_FailTest_Init;
        }


        #region Additional test attributes
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize()
        {
            //using (var db = new MyDbContext())
            //{
            //    if (db.Database.Exists() == true)
            //    {
            //        db.Database.Delete();
            //    }
            //    db.Database.Create();
            //}

            //apply updates 

            Action initAction = null;
            if (initActions.TryGetValue(TestContext.TestName, out initAction) == true)
                initAction();
        }

        // Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        public void MyTestCleanup()
        {
            using (var db = new System.Data.Entity.DbContext("DefaultConnection"))
            {
                if (db.Database.Exists() == true)
                {
                    db.Database.Delete();
                }
            }
            using (var db = new System.Data.Entity.DbContext("Users"))
            {
                if (db.Database.Exists() == true)
                {
                    db.Database.Delete();
                }
            }

        }

        #endregion

        /// <summary>
        /// метод проверяет сохранение пользователя в базе
        /// без категорий классов
        /// </summary>
        [TestMethod]
        public void CreatePerson_SuccessTest()
        {
            try
            {
                //arrange
                using (var db = new CrmDbContext())
                {
                    //var male = TestContext.Properties["GenderType.Male"] as GenderType;
                    var male = db.Set<GenderType>().FirstOrDefault(p => p.Id == GenderType.Male.Id);
                    var married = db.Set<MaritalStatusType>().FirstOrDefault(p => p.Id == MaritalStatusType.Married.Id);

                    var person = new Person("Person 1", "1", male, married);

                    //act 
                    db.Set<Person>().Add(person);
                    db.SaveChanges();

                    //assert
                    var newPerson = db.Set<Person>().FirstOrDefault(p => p.Id == person.Id);
                    Assert.IsNotNull(newPerson);
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    System.Diagnostics.Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }
        private void CreatePerson_SuccessTest_Init()
        {
            //using (var db = new CrmDbContext())
            //{
            //    var male = db.Set<GenderType>().FirstOrDefaultAsync(p => p.Id == GenderType.Male.Id);
            //    TestContext.Properties["GenderType.Male"] = male;
            //}
        }

        [TestMethod]
        public void Login_SuccessTest()
        {
            //AuthenticationManager.SignIn();
            //AuthenticationManager.SignOut();


        }

        [TestMethod]
        public void CreateUser_UsingDBContext_SuccessTest()
        {
            try
            {
                //arrange
                var userName = "Person1";
                var user = new User(userName, "1");
                user.Identities.Add(new PartyIdentity(userName));

                using (var db = new UserDbContext())
                {
                    db.Set<User>().Add(user);
                    db.SaveChanges();

                    var newPerson = db.Set<User>().FirstOrDefault(p => p.Id == user.Id);
                    Assert.IsNotNull(newPerson);
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    System.Diagnostics.Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }
        [TestMethod]
        [ExpectedException(typeof(DbUpdateException))]
        public void CreateUser_UsingDBContext_FailTest()
        {
            try
            {
                //arrange
                var user = new User("Person1", "1");

                using (var db = new UserDbContext())
                {
                    db.Set<User>().Add(user);
                    db.SaveChanges();

                    var newPerson = db.Set<User>().FirstOrDefault(p => p.Id == user.Id);
                    Assert.IsNotNull(newPerson);
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    System.Diagnostics.Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }
        public void CreateUser_UsingDBContext_FailTest_Init()
        {
            try
            {
                //arrange
                var user = new User("Person1", "1");

                using (var db = new UserDbContext())
                {
                    db.Set<User>().Add(user);
                    db.SaveChanges();

                    var newPerson = db.Set<User>().FirstOrDefault(p => p.Id == user.Id);
                    Assert.IsNotNull(newPerson);
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    System.Diagnostics.Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        [TestMethod]
        public void CreateUser_UsingUserManager_SuccessTest()
        {
            try
            {
                //arrange
                var userName = "Person1";
                var user = new User(userName, "1");
                user.Identities.Add(new PartyIdentity(userName));
                //var userManager = new UserManager<ApplicationUser, string>(new UserStore<ApplicationUser>(db));
                var userManager = new UserManager<User, string>(new UserStore());

                ////act 
                var result = userManager.Create<User, string>(user, "123456");
                userManager.Dispose();

                ////assert
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Succeeded == true);

                using (var db = new UserDbContext())
                {
                    var newPerson = db.Set<User>().FirstOrDefault(p => p.Id == user.Id);
                    Assert.IsNotNull(newPerson);
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    System.Diagnostics.Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }
        /// <summary>
        /// то что не выбрасывается сообщение о том что пользователь уже такой есть говорит о том 
        /// что скорее всего если пользователь уже есть с таким идентификатором то ничего не происходит 
        /// </summary>
        [TestMethod]
        //[ExpectedException(typeof(DbUpdateException))]
        public void CreateUser_UsingUserManager_FailTest()
        {
            try
            {
                var userId = TestContext.Properties["UserId"] as string;

                //arrange
                var user = new User("Person1", userId);
                //var userManager = new UserManager<ApplicationUser, string>(new UserStore<ApplicationUser>(db));
                var userManager = new UserManager<User, string>(new UserStore());

                ////act 
                var result = userManager.Create<User, string>(user, "123456");

                ////assert
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Succeeded == true);

                using (var db = new UserDbContext())
                {
                    var newPerson = db.Set<User>().FirstOrDefault(p => p.Id == user.Id);
                    Assert.IsNotNull(newPerson);
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    System.Diagnostics.Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }
        public void CreateUser_UsingUserManager_FailTest_Init()
        {
            var user = new User("Person1", "1");
            //var userManager = new UserManager<ApplicationUser, string>(new UserStore<ApplicationUser>(db));
            var userManager = new UserManager<User, string>(new UserStore());
            ////act 
            var result = userManager.Create<User, string>(user, "123456");

            ////assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Succeeded == true);

            TestContext.Properties["UserId"] = user.Id;
        }

    }
}
