using System.Data.Entity;
using WoaW.Ems.Dal.EF;

namespace WoaW.Tms.DAL.EF.UnitTests
{
    class MyDropCreateDatabaseAlways : DropCreateDatabaseAlways<EmsDbContext>
    {
        protected override void Seed(EmsDbContext context)
        {
            //new DatabaseSeed().Seed(context);

            base.Seed(context);
        }
    }
}
