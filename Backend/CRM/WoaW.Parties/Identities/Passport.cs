using System;

namespace WoaW.CRM.Model.Identities
{
    public sealed class Passport : IdentityDocument
    {
        #region attributes
        #endregion

        #region properties
        #endregion

        #region constructors
        public Passport()
        {

        }
        public Passport(string aNum, string anAuthority, DateTime anIssueDate, DateTime anExpirationDate)
            : this()
        {
            Num = aNum;
            Authority = anAuthority;
            IssueDate = anIssueDate;
            ExpirationDate = anExpirationDate;
        }
        #endregion

    }
}
