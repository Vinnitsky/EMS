using System.Data.Entity;
using WoaW.CMS.Model.Persons;

namespace WoaW.CMS.DAL.EF.Configurations
{
    public class CmsSeed
    {
        public CmsSeed(DbContext dbContext)
        {
            RegisterGenderTypes(dbContext);
            RegisterMaritalStatuses(dbContext);
        }

        private void RegisterMaritalStatuses(DbContext dbContext)
        {
            dbContext.Set<GenderType>().Add(GenderType.Male);
            dbContext.Set<GenderType>().Add(GenderType.Female);
            dbContext.Set<GenderType>().Add(GenderType.NoKnown);
            dbContext.Set<GenderType>().Add(GenderType.NotApplicable);
        }

        private void RegisterGenderTypes(DbContext dbContext)
        {
            dbContext.Set<MaritalStatusType>().Add(MaritalStatusType.NoKnown);
            dbContext.Set<MaritalStatusType>().Add(MaritalStatusType.Married);
            dbContext.Set<MaritalStatusType>().Add(MaritalStatusType.Widowed);
            dbContext.Set<MaritalStatusType>().Add(MaritalStatusType.Separated);
            dbContext.Set<MaritalStatusType>().Add(MaritalStatusType.Divorced);
        }
    }
}
