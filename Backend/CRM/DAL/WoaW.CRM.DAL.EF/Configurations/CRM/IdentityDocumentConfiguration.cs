using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoaW.CMS.Model.Identities;

namespace WoaW.CMS.DAL.EF.Configurations
{
    class IdentityDocumentConfiguration : EntityTypeConfiguration<IdentityDocument>
    {
        public IdentityDocumentConfiguration()
        {
            Property(t => t.Num).IsRequired();
            Property(t => t.IssueDate).IsRequired();
            Property(t => t.IssueDate).HasColumnType("datetime2");
            Property(t => t.ExpirationDate).HasColumnType("datetime2");

        }
    }
}
