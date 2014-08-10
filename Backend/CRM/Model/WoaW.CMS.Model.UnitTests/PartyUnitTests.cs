using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using WoaW.CMS.Model.Persons;

namespace WoaW.CMS.Model.UnitTests
{
    [TestClass]
    public class PartyUnitTests
    {
        /// <summary>
        /// передамем пустой ID в персону - должено быть исключение
        /// </summary>
        [TestMethod]
        [TestCategory("CRM.Party")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Person_EmptyId_FailTest()
        {
            var person1 = new Person("Person1", "");
        }
        /// <summary>
        /// передамем пустой Title в персону - должено быть исключение
        /// </summary>
        [TestMethod]
        [TestCategory("CRM.Party")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Person_EmptyTitle_FailTest()
        {
            var person1 = new Person("", "1");
        }
        /// <summary>
        /// определяем человека - мужского пола
        /// </summary>
        [TestMethod]
        [TestCategory("CRM.Party")]
        public void Define_Male_SuccessTest()
        {
            var person1 = new Person("Person1", "1", GenderType.Male);
        }
        /// <summary>
        /// определяем человека, мужского пола, женатого
        /// </summary>
        [TestMethod]
        [TestCategory("CRM.Party")]
        public void Define_Male_Married_SuccessTest()
        {
            var person1 = new Person("Person1", "1", GenderType.Male);
            person1.MeritalStatuses.Add(new MaritalStatus(MaritalStatusType.Married, new DateTime(1900, 1, 1)));
        }
        /// <summary>
        /// получаем из списка людей человека мужского пола
        /// </summary>
        [TestMethod]
        [TestCategory("CRM.Party")]
        public void Get_Male_FromList_SuccessTest()
        {
            var person1 = new Person("Person1", "1", GenderType.Male);
            var person2 = new Person("Person2", "2", GenderType.Female);
            var people = new List<Person>() { person1, person2 };

            var gender = person1.Gender;
            Assert.AreEqual(gender, GenderType.Male);

            //var p3 = people.SingleOrDefault(p => p.Gender.Equals(Gender.Male));
            //var p3 = people.SingleOrDefault(p => p.Gender == Gender.Male);
            var p3 = people.SingleOrDefault(p => p.Gender.Id == GenderType.Male.Id);
            Assert.IsNotNull(p3);
            Assert.AreEqual(person1, p3);

            //var p4 = people.SingleOrDefault(p => p.Genders.Any(g=>g.Type == Gender.Male));
            var p4 = people.SingleOrDefault(p => p.Genders.Any(g => g.Type.Id == GenderType.Male.Id));
            Assert.IsNotNull(p4);
            Assert.AreEqual(person1, p4);
        }
        /// <summary>
        /// проверяем на равенство значения полов
        /// </summary>
        [TestMethod]
        [TestCategory("CRM.Party")]
        public void ValidateGenderTypes_SuccessTest()
        {
            var male1 = GenderType.Male;
            var male2 = GenderType.Female;
            var male3 = GenderType.Male;

            Assert.AreEqual(male1, GenderType.Male);
            Assert.AreEqual(male1, male3);
        }
    }
}
