﻿using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoaW.CRM.Model;

namespace WoaW.CRM.DAL.EF
{
    public class PartyIdentity : IdentityUser
    {
        public PartyIdentity()
        {

        }
        public PartyIdentity(string userName)
            : base(userName)
        {

        }

        public string UserId { get; set; }
        virtual public User User { get; set; }
    }
}
