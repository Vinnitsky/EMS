using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raven.Client.Document;
using WoaW.CMS.Model.Persons;

namespace WoaW.CMS.DAL.Ravendb.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        readonly IDictionary<string, Action> _initActions = new Dictionary<string, Action>();

        public UnitTest1()
        {
            //_initActions["CreatePerson_SuccessTest"] = CreatePerson_SuccessTest_Init;
        }

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
        [TestInitialize]
        public void MyTestInitialize()
        {
            Action action;
            if (_initActions.TryGetValue(TestContext.TestName, out action))
                action();
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup]
        public void TestCleanup()
        {
        }

        #endregion

        /// <summary>
        /// определяем человека - мужского пола
        /// </summary>
        [TestMethod]
        [TestCategory("RavenDB.CRM.Party")]
        public void Define_Male_SuccessTest()
        {
            using (var store = new DocumentStore { ConnectionStringName = "ravenDB" }.Initialize())
            {
                using (var session = store.OpenSession("WoaW.Raven.Ems"))
                {
                    var person1 = new Person("Person1", "1", GenderType.Male);
                    session.Store(person1);
                    session.SaveChanges();
                }
            }
        }
        /// <summary>
        /// определяем человека, мужского пола, женатого
        /// </summary>
        [TestMethod]
        [TestCategory("RavenDB.CRM.Party")]
        public void Define_Male_Married_SuccessTest()
        {
            using (var store = new DocumentStore { ConnectionStringName = "ravenDB" }.Initialize())
            {
                using (var session = store.OpenSession("WoaW.Raven.Ems")) 
                {
                    var person1 = new Person("Person1", "1", GenderType.Male);
                    person1.MeritalStatuses.Add(new MaritalStatus(MaritalStatusType.Married, new DateTime(1900, 1, 1)));
                }
            }
        }
    }
}
