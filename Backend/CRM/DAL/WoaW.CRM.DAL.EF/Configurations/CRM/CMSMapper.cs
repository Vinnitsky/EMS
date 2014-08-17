using System.Data.Entity;

namespace WoaW.CMS.DAL.EF.Configurations
{
    public class CmsMapper
    {
        public CmsMapper(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new PartyConfiguration());
            modelBuilder.Configurations.Add(new PersonConfiguration());
            modelBuilder.Configurations.Add(new PersonNameConfigurations());
            modelBuilder.Configurations.Add(new PersonNameOptionConfigurations());
            modelBuilder.Configurations.Add(new PersonNameTypeConfigurations());
            modelBuilder.Configurations.Add(new OrganiazationConfiguration());
            modelBuilder.Configurations.Add(new PartyClasificationConfiguration());
            modelBuilder.Configurations.Add(new PartyRelationshipConfiguration());
            modelBuilder.Configurations.Add(new RuleSetConfiguration());
            modelBuilder.Configurations.Add(new GenderConfiguration());
            modelBuilder.Configurations.Add(new IdentificationConfiguration());
            modelBuilder.Configurations.Add(new IdentityDocumentConfiguration());
            modelBuilder.Configurations.Add(new MaritalStatusTypeConfiguration());
            modelBuilder.Configurations.Add(new MaritalStatusConfiguration());
            modelBuilder.Configurations.Add(new PhysicalCharacteristicConfiguration());
            modelBuilder.Configurations.Add(new PhisicalCharacteristicOptionConfiguration());
        }
    }

}
