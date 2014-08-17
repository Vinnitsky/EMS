using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using WoaW.CMS.Model.Persons;
using System.Data.Entity.Validation;
using System.Data.Entity;
using WoaW.Ems.Dal.EF;

namespace WoaW.CMS.DAL.EF.UnitTests
{
    [TestClass]
    public class PersonUnitTest
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

        public PersonUnitTest()
        {
            Database.SetInitializer(new MyDropCreateDatabaseAlways());

            _initActions["CreatePerson_SuccessTest"] = CreatePerson_SuccessTest_Init;
        }


        /// <summary>
        /// метод проверяет сохранение пользователя в базе
        /// без категорий классов
        /// </summary>
        [TestMethod]
        [TestCategory("CMS.Party.EF")]
        public void CreatePerson_SuccessTest()
        {
            try
            {
                //arrange
                    //var male = TestContext.Properties["GenderType.Male"] as GenderType;
                    var male = Context.Set<GenderType>().FirstOrDefault(p => p.Id == GenderType.Male.Id);
                    var married = Context.Set<MaritalStatusType>().FirstOrDefault(p => p.Id == MaritalStatusType.Married.Id);

                    var person = new Person("Person 1", "1", male, married);

                    //act 
                    Context.Set<Person>().Add(person);
                    Context.SaveChanges();

                    //assert
                    var newPerson = Context.Set<Person>().FirstOrDefault(p => p.Id == person.Id);
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
        private void CreatePerson_SuccessTest_Init()
        {
            //using (var db = new CrmDbContext())
            //{
            //    var male = db.Set<GenderType>().FirstOrDefaultAsync(p => p.Id == GenderType.Male.Id);
            //    TestContext.Properties["GenderType.Male"] = male;
            //}
        }
    }
}
