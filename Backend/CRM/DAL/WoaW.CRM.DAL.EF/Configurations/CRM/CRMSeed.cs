using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoaW.CMS.Model.Persons;

namespace WoaW.CMS.DAL.EF.Configurations
{
    class CRMSeed
    {
        public CRMSeed(CrmDbContext dbContext)
        {
            RegisterGenderTypes(dbContext);
            RegisterMaritalStatuses(dbContext);
        }

        private void RegisterMaritalStatuses(CrmDbContext dbContext)
        {
            dbContext.Set<GenderType>().Add(GenderType.Male);
            dbContext.Set<GenderType>().Add(GenderType.Female);
            dbContext.Set<GenderType>().Add(GenderType.NoKnown);
            dbContext.Set<GenderType>().Add(GenderType.NotApplicable);
        }

        private void RegisterGenderTypes(CrmDbContext dbContext)
        {
            dbContext.Set<MaritalStatusType>().Add(MaritalStatusType.NoKnown);
            dbContext.Set<MaritalStatusType>().Add(MaritalStatusType.Married);
            dbContext.Set<MaritalStatusType>().Add(MaritalStatusType.Widowed);
            dbContext.Set<MaritalStatusType>().Add(MaritalStatusType.Separated);
            dbContext.Set<MaritalStatusType>().Add(MaritalStatusType.Divorced);
        }
    }
}
