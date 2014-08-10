using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WoaW.CRM.Model;

namespace WoaW.CRM.DAL.EF
{
    public class User : Party, IUser<string>
    {
        public User()
        {
            Identities = new List<PartyIdentity>();
        }
        public User(string title, string id)
            : base(title, id)
        {
            Identities = new List<PartyIdentity>();
        }
        public string UserName
        {
            get
            {
                return base.Title;
            }
            set
            {
                base.Title = value;
            }
        }

        public string PasswordHash { get; set; }

        virtual public List<PartyIdentity> Identities { get; set; }
    }
}
