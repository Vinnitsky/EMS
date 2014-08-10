using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WoaW.CRM.Model.Persons;
using WoaW.CRM.Model.Repationships;

namespace WoaW.CRM.Model.UnitTests
{
    [TestClass]
    public class RelationshipUnitTest
    {
        /// <summary>
        /// передаем два сотрудника с двумя правильными ролями
        /// </summary>
        [TestMethod]
        [TestCategory("CRM.Relationship")]
        public void Define_Employment_Relationship_SucessTest()
        {
            var person1 = new Person("Person1", "1");
            var person2 = new Person("Person2", "2");

            var employerRole = new RoleType("Employer", "1");
            var employeeRole1 = new RoleType("EMployeeRole1", "2");
            var manager = new PartyRole(employerRole, person1);
            var worker1 = new PartyRole(employeeRole1, person2);

            var employmentRelationshipType = new RelationshipType("Employment", new RoleType[] { employerRole, employeeRole1 }, "1");
            var employment = new PartyRelationship(employmentRelationshipType, new PartyRole[] { manager, worker1 });
        }

        /// <summary>
        /// передаем три сотрудника - с тремя разными ролями, 
        /// отношение поддерживает только две роли - значит должна быть ошибка
        /// </summary>
        [TestMethod]
        [TestCategory("CRM.Relationship")]
        [ExpectedException(typeof(ArgumentException))]
        public void Define_Employment_Relationship_FeilTest()
        {
            var person1 = new Person("Person1", "1");
            var person2 = new Person("Person2", "2");
            var person3 = new Person("Person3", "3");

            var employerRole = new RoleType("Employer", "1");
            var employeeRole1 = new RoleType("EMployeeRole1", "2");
            var employeeRole2 = new RoleType("EMployeeRole2", "3");
            var manager = new PartyRole(employerRole, person1);
            var worker1 = new PartyRole(employeeRole1, person2);
            var worker2 = new PartyRole(employeeRole2, person2);

            var employmentRelationshipType = new RelationshipType("Employment", new RoleType[] { employerRole, employeeRole1 }, "1");
            var employment = new PartyRelationship(employmentRelationshipType, new PartyRole[] { manager, worker1, worker2 });
        }

        /// <summary>
        /// передамем две роли одна из котороых пустая - не содержит сотрудника 
        /// должна быть ошибка
        /// </summary>
        [TestMethod]
        [TestCategory("CRM.Relationship")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PartyRole_EmptyParty_FailTest()
        {
            var person1 = new Person("Person1", "1");
            var person2 = new Person("Person2", "2");

            var employerRole = new RoleType("Employer", "1");
            var employeeRole1 = new RoleType("EMployeeRole1", "2");
            var manager = new PartyRole(employerRole, person1);
            var worker1 = new PartyRole(employeeRole1, null);
        }
        /// <summary>
        /// передамем две роли одна из котороых пустая - не содержит сотрудника 
        /// должна быть ошибка
        /// </summary>
        [TestMethod]
        [TestCategory("CRM.Relationship")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PartyRole_EmptyRoleType_FailTest()
        {
            var person1 = new Person("Person1", "1");
            var person2 = new Person("Person2", "2");

            var employerRole = new RoleType("Employer", "1");
            var employeeRole1 = new RoleType("EMployeeRole1", "2");
            var manager = new PartyRole(employerRole, person1);
            var worker1 = new PartyRole(null, person2);
        }
    }
}
