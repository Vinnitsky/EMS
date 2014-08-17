using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoaW.CMS.Model
{
    public interface IIdentity
    {
        string Id { get; set; }
        Party Party { get; set; }
    }
}
