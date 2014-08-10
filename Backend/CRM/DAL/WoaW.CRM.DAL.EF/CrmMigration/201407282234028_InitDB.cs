namespace WoaW.CMS.DAL.EF.CrmMigration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Party",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Title = c.String(nullable: false),
                        Description = c.String(),
                        LockedByTaskId = c.String(),
                        FromDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ThruDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Version = c.String(),
                        FederalTaxIdNum = c.String(),
                        Discriminator = c.String(maxLength: 128),
                        Signature_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Signature", t => t.Signature_Id)
                .Index(t => t.Signature_Id);
            
            CreateTable(
                "dbo.Identification",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Title = c.String(nullable: false),
                        Description = c.String(),
                        ValidFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidTo = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Party_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Party", t => t.Party_Id)
                .Index(t => t.Party_Id);
            
            CreateTable(
                "dbo.Signature",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Description = c.String(),
                        Image = c.Binary(),
                        ValidFrom = c.DateTime(nullable: false),
                        ValidTo = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Citizenship",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Title = c.String(),
                        ForCountry_Title = c.String(),
                        ForCountry_Iso2Name = c.String(),
                        ForCountry_Iso3Name = c.String(),
                        Person_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Person", t => t.Person_Id)
                .Index(t => t.Person_Id);
            
            CreateTable(
                "dbo.IdentityDocument",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Num = c.String(nullable: false),
                        Title = c.String(),
                        IssueDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ExpirationDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Authority = c.String(),
                        ForCountry_Title = c.String(),
                        ForCountry_Iso2Name = c.String(),
                        ForCountry_Iso3Name = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Citizenship_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Citizenship", t => t.Citizenship_Id)
                .Index(t => t.Citizenship_Id);
            
            CreateTable(
                "dbo.PageScan",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        IdentityDocument_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IdentityDocument", t => t.IdentityDocument_Id)
                .Index(t => t.IdentityDocument_Id);
            
            CreateTable(
                "dbo.Gender",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FromDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ThruDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Type_Id = c.String(maxLength: 128),
                        Person_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GenderType", t => t.Type_Id)
                .ForeignKey("dbo.Person", t => t.Person_Id)
                .Index(t => t.Type_Id)
                .Index(t => t.Person_Id);
            
            CreateTable(
                "dbo.GenderType",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Title = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MaritalStatus",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FromDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        ThruDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Type_Id = c.String(maxLength: 128),
                        Person_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MaritalStatusType", t => t.Type_Id)
                .ForeignKey("dbo.Person", t => t.Person_Id)
                .Index(t => t.Type_Id)
                .Index(t => t.Person_Id);
            
            CreateTable(
                "dbo.MaritalStatusType",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Title = c.String(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PersonName",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                        FromDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        ThruDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        SelectedOption_Id = c.String(maxLength: 128),
                        Type_Id = c.String(maxLength: 128),
                        Person_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PersonNameOption", t => t.SelectedOption_Id)
                .ForeignKey("dbo.PersonNameType", t => t.Type_Id)
                .ForeignKey("dbo.Person", t => t.Person_Id)
                .Index(t => t.SelectedOption_Id)
                .Index(t => t.Type_Id)
                .Index(t => t.Person_Id);
            
            CreateTable(
                "dbo.PersonNameOption",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Value = c.String(nullable: false),
                        ForPersonNameType_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PersonNameType", t => t.ForPersonNameType_Id)
                .Index(t => t.ForPersonNameType_Id);
            
            CreateTable(
                "dbo.PersonNameType",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Title = c.String(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PhysicalCharacteristic",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FromDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ThruDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Value = c.String(nullable: false),
                        Type_Id = c.String(maxLength: 128),
                        Person_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PhysicalCharacteristicType", t => t.Type_Id)
                .ForeignKey("dbo.Person", t => t.Person_Id)
                .Index(t => t.Type_Id)
                .Index(t => t.Person_Id);
            
            CreateTable(
                "dbo.PhisicalCharacteristicOption",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Value = c.String(nullable: false),
                        ForPhysicalCharacteristicType_Id = c.String(maxLength: 128),
                        PhysicalCharacteristic_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PhysicalCharacteristicType", t => t.ForPhysicalCharacteristicType_Id)
                .ForeignKey("dbo.PhysicalCharacteristic", t => t.PhysicalCharacteristic_Id)
                .Index(t => t.ForPhysicalCharacteristicType_Id)
                .Index(t => t.PhysicalCharacteristic_Id);
            
            CreateTable(
                "dbo.PhysicalCharacteristicType",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Title = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PartyClasification",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FromDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ThruDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        PartyType_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PartyType", t => t.PartyType_Id)
                .Index(t => t.PartyType_Id);
            
            CreateTable(
                "dbo.PartyType",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Title = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PartyRelationship",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Comment = c.String(),
                        FromDate = c.DateTime(nullable: false),
                        ThruDate = c.DateTime(),
                        Type_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RelationshipType", t => t.Type_Id)
                .Index(t => t.Type_Id);
            
            CreateTable(
                "dbo.PartyRole",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FromDate = c.DateTime(nullable: false),
                        ThruDate = c.DateTime(),
                        Party_Id = c.String(maxLength: 128),
                        Type_Id = c.String(maxLength: 128),
                        PartyRelationship_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Party", t => t.Party_Id)
                .ForeignKey("dbo.RoleType", t => t.Type_Id)
                .ForeignKey("dbo.PartyRelationship", t => t.PartyRelationship_Id)
                .Index(t => t.Party_Id)
                .Index(t => t.Type_Id)
                .Index(t => t.PartyRelationship_Id);
            
            CreateTable(
                "dbo.RoleType",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Title = c.String(),
                        Description = c.String(),
                        RelationshipType_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RelationshipType", t => t.RelationshipType_Id)
                .Index(t => t.RelationshipType_Id);
            
            CreateTable(
                "dbo.RelationshipType",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Title = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RuleSet",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        AtributeTitle = c.String(),
                        AtributeValue = c.String(),
                        Description = c.String(),
                        FromDate = c.DateTime(nullable: false),
                        ThruDate = c.DateTime(nullable: false),
                        Title = c.String(),
                        Relationship_Id = c.String(maxLength: 128),
                        Role_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RelationshipType", t => t.Relationship_Id)
                .ForeignKey("dbo.RoleType", t => t.Role_Id)
                .Index(t => t.Relationship_Id)
                .Index(t => t.Role_Id);
            
            CreateTable(
                "dbo.RelationshipConstraine",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Description = c.String(),
                        FromDate = c.DateTime(nullable: false),
                        ThruDate = c.DateTime(nullable: false),
                        Title = c.String(),
                        Role_Id = c.String(maxLength: 128),
                        RuleSet_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RoleType", t => t.Role_Id)
                .ForeignKey("dbo.RuleSet", t => t.RuleSet_Id)
                .Index(t => t.Role_Id)
                .Index(t => t.RuleSet_Id);
            
            CreateTable(
                "dbo.Person",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        SSID = c.String(),
                        Birthdate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Party", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Organization",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Party", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Organization", "Id", "dbo.Party");
            DropForeignKey("dbo.Person", "Id", "dbo.Party");
            DropForeignKey("dbo.RuleSet", "Role_Id", "dbo.RoleType");
            DropForeignKey("dbo.RuleSet", "Relationship_Id", "dbo.RelationshipType");
            DropForeignKey("dbo.RelationshipConstraine", "RuleSet_Id", "dbo.RuleSet");
            DropForeignKey("dbo.RelationshipConstraine", "Role_Id", "dbo.RoleType");
            DropForeignKey("dbo.PartyRelationship", "Type_Id", "dbo.RelationshipType");
            DropForeignKey("dbo.RoleType", "RelationshipType_Id", "dbo.RelationshipType");
            DropForeignKey("dbo.PartyRole", "PartyRelationship_Id", "dbo.PartyRelationship");
            DropForeignKey("dbo.PartyRole", "Type_Id", "dbo.RoleType");
            DropForeignKey("dbo.PartyRole", "Party_Id", "dbo.Party");
            DropForeignKey("dbo.PartyClasification", "PartyType_Id", "dbo.PartyType");
            DropForeignKey("dbo.PhysicalCharacteristic", "Person_Id", "dbo.Person");
            DropForeignKey("dbo.PhysicalCharacteristic", "Type_Id", "dbo.PhysicalCharacteristicType");
            DropForeignKey("dbo.PhisicalCharacteristicOption", "PhysicalCharacteristic_Id", "dbo.PhysicalCharacteristic");
            DropForeignKey("dbo.PhisicalCharacteristicOption", "ForPhysicalCharacteristicType_Id", "dbo.PhysicalCharacteristicType");
            DropForeignKey("dbo.PersonName", "Person_Id", "dbo.Person");
            DropForeignKey("dbo.PersonName", "Type_Id", "dbo.PersonNameType");
            DropForeignKey("dbo.PersonName", "SelectedOption_Id", "dbo.PersonNameOption");
            DropForeignKey("dbo.PersonNameOption", "ForPersonNameType_Id", "dbo.PersonNameType");
            DropForeignKey("dbo.MaritalStatus", "Person_Id", "dbo.Person");
            DropForeignKey("dbo.MaritalStatus", "Type_Id", "dbo.MaritalStatusType");
            DropForeignKey("dbo.Gender", "Person_Id", "dbo.Person");
            DropForeignKey("dbo.Gender", "Type_Id", "dbo.GenderType");
            DropForeignKey("dbo.Citizenship", "Person_Id", "dbo.Person");
            DropForeignKey("dbo.IdentityDocument", "Citizenship_Id", "dbo.Citizenship");
            DropForeignKey("dbo.PageScan", "IdentityDocument_Id", "dbo.IdentityDocument");
            DropForeignKey("dbo.Party", "Signature_Id", "dbo.Signature");
            DropForeignKey("dbo.Identification", "Party_Id", "dbo.Party");
            DropIndex("dbo.Organization", new[] { "Id" });
            DropIndex("dbo.Person", new[] { "Id" });
            DropIndex("dbo.RelationshipConstraine", new[] { "RuleSet_Id" });
            DropIndex("dbo.RelationshipConstraine", new[] { "Role_Id" });
            DropIndex("dbo.RuleSet", new[] { "Role_Id" });
            DropIndex("dbo.RuleSet", new[] { "Relationship_Id" });
            DropIndex("dbo.RoleType", new[] { "RelationshipType_Id" });
            DropIndex("dbo.PartyRole", new[] { "PartyRelationship_Id" });
            DropIndex("dbo.PartyRole", new[] { "Type_Id" });
            DropIndex("dbo.PartyRole", new[] { "Party_Id" });
            DropIndex("dbo.PartyRelationship", new[] { "Type_Id" });
            DropIndex("dbo.PartyClasification", new[] { "PartyType_Id" });
            DropIndex("dbo.PhisicalCharacteristicOption", new[] { "PhysicalCharacteristic_Id" });
            DropIndex("dbo.PhisicalCharacteristicOption", new[] { "ForPhysicalCharacteristicType_Id" });
            DropIndex("dbo.PhysicalCharacteristic", new[] { "Person_Id" });
            DropIndex("dbo.PhysicalCharacteristic", new[] { "Type_Id" });
            DropIndex("dbo.PersonNameOption", new[] { "ForPersonNameType_Id" });
            DropIndex("dbo.PersonName", new[] { "Person_Id" });
            DropIndex("dbo.PersonName", new[] { "Type_Id" });
            DropIndex("dbo.PersonName", new[] { "SelectedOption_Id" });
            DropIndex("dbo.MaritalStatus", new[] { "Person_Id" });
            DropIndex("dbo.MaritalStatus", new[] { "Type_Id" });
            DropIndex("dbo.Gender", new[] { "Person_Id" });
            DropIndex("dbo.Gender", new[] { "Type_Id" });
            DropIndex("dbo.PageScan", new[] { "IdentityDocument_Id" });
            DropIndex("dbo.IdentityDocument", new[] { "Citizenship_Id" });
            DropIndex("dbo.Citizenship", new[] { "Person_Id" });
            DropIndex("dbo.Identification", new[] { "Party_Id" });
            DropIndex("dbo.Party", new[] { "Signature_Id" });
            DropTable("dbo.Organization");
            DropTable("dbo.Person");
            DropTable("dbo.RelationshipConstraine");
            DropTable("dbo.RuleSet");
            DropTable("dbo.RelationshipType");
            DropTable("dbo.RoleType");
            DropTable("dbo.PartyRole");
            DropTable("dbo.PartyRelationship");
            DropTable("dbo.PartyType");
            DropTable("dbo.PartyClasification");
            DropTable("dbo.PhysicalCharacteristicType");
            DropTable("dbo.PhisicalCharacteristicOption");
            DropTable("dbo.PhysicalCharacteristic");
            DropTable("dbo.PersonNameType");
            DropTable("dbo.PersonNameOption");
            DropTable("dbo.PersonName");
            DropTable("dbo.MaritalStatusType");
            DropTable("dbo.MaritalStatus");
            DropTable("dbo.GenderType");
            DropTable("dbo.Gender");
            DropTable("dbo.PageScan");
            DropTable("dbo.IdentityDocument");
            DropTable("dbo.Citizenship");
            DropTable("dbo.Signature");
            DropTable("dbo.Identification");
            DropTable("dbo.Party");
        }
    }
}
