using System.Data.Entity;

namespace WoaW.CMS.DAL.EF.Configurations
{
    public sealed class IdentityMapper
    {
        public IdentityMapper(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new IdentificationConfiguration());
            modelBuilder.Configurations.Add(new SignatureConfiguration());
        }
    }

}
