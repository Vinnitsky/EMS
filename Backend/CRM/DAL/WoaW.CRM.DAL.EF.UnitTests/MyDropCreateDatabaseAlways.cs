using System.Data.Entity;
using WoaW.Ems.Dal.EF;

namespace WoaW.CMS.DAL.EF.UnitTests
{
    class MyDropCreateDatabaseAlways : DropCreateDatabaseAlways<CrmDbContext>
    {
        protected override void Seed(CrmDbContext context)
        {
            //new DatabaseSeed().Seed(context);

            base.Seed(context);
        }
    }
}
