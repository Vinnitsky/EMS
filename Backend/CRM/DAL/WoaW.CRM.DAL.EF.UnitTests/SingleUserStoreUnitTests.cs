using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using WoaW.Ems.Dal.EF;

namespace WoaW.CMS.DAL.EF.UnitTests
{
    /// <summary>
    /// Summary description for UnitTest2
    /// </summary>
    [TestClass]
    public class SingleUserStoreUnitTests
    {
        readonly IDictionary<string, Action> _initActions = new Dictionary<string, Action>();
        DbContext Context { get; set; }
        #region Additional test attributes

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        // Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize()
        {
            try
            {
                Context = new EmsDbContext();

                Action action = null;
                if (_initActions.TryGetValue(TestContext.TestName, out action) == true)
                    action();
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

        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void TestCleanup()
        {
            Context.Dispose();

            using (var db = new EmsDbContext())
            {
                if (db.Database.Exists())
                    db.Database.Delete();
            }
        }

        #endregion

        public SingleUserStoreUnitTests()
        {
            Database.SetInitializer(new MyDropCreateDatabaseAlways());

            try
            {
                _initActions["CreateUser_UsingDBContext_FailTest"] = CreateUser_UsingDBContext_FailTest_Init;

                _initActions["CreateUser_UsingUserManager_FailTest"] = CreateUser_UsingUserManager_FailTest_Init;
                _initActions["CreateUser_UsingUserManager_FailTest"] = CreateUser_UsingUserManager_FailTest_Init;
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
        [TestCategory("CMS.Party.EF.UserStore")]
        public void Login_SuccessTest()
        {
            //AuthenticationManager.SignIn();
            //AuthenticationManager.SignOut();
        }

        [TestMethod]
        [TestCategory("CMS.Party.EF.UserStore")]
        public void CreateUser_UsingDBContext_SuccessTest()
        {
            try
            {
                //arrange
                var userName = "Person1";
                var user = new PartyIdentity(userName, "1");

                Context.Set<PartyIdentity>().Add(user);
                Context.SaveChanges();

                var newPerson = Context.Set<PartyIdentity>().FirstOrDefault(p => p.Id == user.Id);
                Assert.IsNotNull(newPerson);
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
        /// метод создает идентити с таким идентификатором который уже существует в базе
        /// </summary>
        [TestMethod]
        [TestCategory("CMS.Party.EF.UserStore")]
        [ExpectedException(typeof(DbEntityValidationException))]
        public void CreateUser_UsingDBContext_FailTest()
        {
            try
            {
                //arrange
                var user = new PartyIdentity("Person1", "1");

                Context.Set<PartyIdentity>().Add(user);
                Context.SaveChanges();

                var newPerson = Context.Set<PartyIdentity>().FirstOrDefault(p => p.Id == user.Id);
                Assert.IsNotNull(newPerson);
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
                var user = new PartyIdentity("Person1", "1");

                Context.Set<PartyIdentity>().Add(user);
                Context.SaveChanges();

                var newPerson = Context.Set<PartyIdentity>().FirstOrDefault(p => p.Id == user.Id);
                Assert.IsNotNull(newPerson);
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
        [TestCategory("CMS.Party.EF.UserStore")]
        public void CreateUser_UsingUserManager_SuccessTest()
        {
            try
            {
                //arrange
                var userName = "Person1";
                var user = new PartyIdentity(userName, "1");
                var userManager = new UserManager<PartyIdentity, string>(new SingleUserStore(Context));

                ////act 
                var result = userManager.Create<PartyIdentity, string>(user, "123456");
                userManager.Dispose();

                ////assert
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Succeeded == true);

                var newPerson = Context.Set<PartyIdentity>().FirstOrDefault(p => p.Id == user.Id);
                Assert.IsNotNull(newPerson);
                Assert.IsNotNull(newPerson.Party);
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
        [TestCategory("CMS.Party.EF.UserStore")]
        [ExpectedException(typeof(DbEntityValidationException))]
        public void CreateUser_UsingUserManager_FailTest()
        {
            try
            {
                var userId = TestContext.Properties["UserId"] as string;

                //arrange
                var user = new PartyIdentity("Person1", userId);
                var userManager = new UserManager<PartyIdentity, string>(new SingleUserStore(Context));

                ////act 
                var result = userManager.Create<PartyIdentity, string>(user, "123456");

                ////assert
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Succeeded == true);

                var newPerson = Context.Set<PartyIdentity>().FirstOrDefault(p => p.Id == user.Id);
                Assert.IsNotNull(newPerson);
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
            var user = new PartyIdentity("Person1", "1");
            var userManager = new UserManager<PartyIdentity, string>(new SingleUserStore(Context));
            ////act 
            var result = userManager.Create<PartyIdentity, string>(user, "123456");

            ////assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Succeeded == true);

            TestContext.Properties["UserId"] = user.Id;
        }

    }
}
