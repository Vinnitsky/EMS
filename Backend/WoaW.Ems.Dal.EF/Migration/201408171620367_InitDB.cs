namespace WoaW.Ems.Dal.EF.Migration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(precision: 7, storeType: "datetime2"),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Fio = c.String(),
                        IsDisabled = c.Boolean(),
                        IsOnline = c.Boolean(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Party_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Party", t => t.Party_Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.Party_Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
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
                        IsBusy = c.Boolean(nullable: false),
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
                        ValidFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidTo = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
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
                        FromDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ThruDate = c.DateTime(precision: 7, storeType: "datetime2"),
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
                        FromDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ThruDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsBussy = c.Boolean(),
                        IsShared = c.Boolean(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
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
                "dbo.WorkEffortRate",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        From = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Thru = c.DateTime(precision: 7, storeType: "datetime2"),
                        Rate = c.Single(nullable: false),
                        Comment = c.String(),
                        Type = c.Int(nullable: false),
                        EmployeeRole_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PartyRole", t => t.EmployeeRole_Id)
                .Index(t => t.EmployeeRole_Id);
            
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
                        FromDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ThruDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
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
                        FromDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ThruDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
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
                "dbo.WorkEffort",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        AllowManualAcceptance = c.Boolean(nullable: false),
                        CreationTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        EstimatedTime = c.Time(precision: 7),
                        ActualTime = c.Time(precision: 7),
                        TotalHoursAllowed = c.Time(precision: 7),
                        ScheduledStartTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        DueDateTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        DueFinishTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        ActualStartTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        ActualFinishTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        Priority = c.Int(nullable: false),
                        DataOjectId = c.String(),
                        Result = c.Int(),
                        Subject = c.String(),
                        Text = c.String(),
                        Status = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        AssignedToParty_Id = c.String(maxLength: 128),
                        RequerdRole_Id = c.String(maxLength: 128),
                        Type_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AssignedToParty_Id)
                .ForeignKey("dbo.RoleType", t => t.RequerdRole_Id)
                .ForeignKey("dbo.WorkEffortType", t => t.Type_Id)
                .Index(t => t.AssignedToParty_Id)
                .Index(t => t.RequerdRole_Id)
                .Index(t => t.Type_Id);
            
            CreateTable(
                "dbo.WorkEffortType",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Title = c.String(nullable: false, maxLength: 256),
                        Description = c.String(),
                        Class = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        IsSystem = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WorkEffortAssociation",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        TypeOfAssociatedWorkEffort_Id = c.String(maxLength: 128),
                        WorkEffort_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WorkEffortType", t => t.TypeOfAssociatedWorkEffort_Id)
                .ForeignKey("dbo.WorkEffort", t => t.WorkEffort_Id)
                .Index(t => t.TypeOfAssociatedWorkEffort_Id)
                .Index(t => t.WorkEffort_Id);
            
            CreateTable(
                "dbo.WorkEffortHistorycalRecord",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        TaskId = c.String(nullable: false),
                        ManagerId = c.String(),
                        EmployeeId = c.String(),
                        Description = c.String(),
                        Time = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Status = c.Int(),
                        OperationComment = c.String(),
                        WorkEffortPartyAssignment_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WorkEffortPartyAssignment", t => t.WorkEffortPartyAssignment_Id)
                .Index(t => t.WorkEffortPartyAssignment_Id);
            
            CreateTable(
                "dbo.WorkEffortPartyAssignment",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AssignedAt = c.DateTime(precision: 7, storeType: "datetime2"),
                        AcceptedAt = c.DateTime(precision: 7, storeType: "datetime2"),
                        RejectedAt = c.DateTime(precision: 7, storeType: "datetime2"),
                        CanceleddAt = c.DateTime(precision: 7, storeType: "datetime2"),
                        ClosedAt = c.DateTime(precision: 7, storeType: "datetime2"),
                        Status = c.Int(nullable: false),
                        AssignedTo_Id = c.String(maxLength: 128),
                        WorkEffort_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PartyRole", t => t.AssignedTo_Id)
                .ForeignKey("dbo.WorkEffort", t => t.WorkEffort_Id, cascadeDelete: true)
                .Index(t => t.AssignedTo_Id)
                .Index(t => t.WorkEffort_Id);
            
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
            DropForeignKey("dbo.WorkEffortPartyAssignment", "WorkEffort_Id", "dbo.WorkEffort");
            DropForeignKey("dbo.WorkEffortAssociation", "WorkEffort_Id", "dbo.WorkEffort");
            DropForeignKey("dbo.WorkEffort", "Type_Id", "dbo.WorkEffortType");
            DropForeignKey("dbo.WorkEffort", "RequerdRole_Id", "dbo.RoleType");
            DropForeignKey("dbo.WorkEffortHistorycalRecord", "WorkEffortPartyAssignment_Id", "dbo.WorkEffortPartyAssignment");
            DropForeignKey("dbo.WorkEffortPartyAssignment", "AssignedTo_Id", "dbo.PartyRole");
            DropForeignKey("dbo.WorkEffortAssociation", "TypeOfAssociatedWorkEffort_Id", "dbo.WorkEffortType");
            DropForeignKey("dbo.WorkEffort", "AssignedToParty_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Party_Id", "dbo.Party");
            DropForeignKey("dbo.RuleSet", "Role_Id", "dbo.RoleType");
            DropForeignKey("dbo.RuleSet", "Relationship_Id", "dbo.RelationshipType");
            DropForeignKey("dbo.RelationshipConstraine", "RuleSet_Id", "dbo.RuleSet");
            DropForeignKey("dbo.RelationshipConstraine", "Role_Id", "dbo.RoleType");
            DropForeignKey("dbo.PartyRelationship", "Type_Id", "dbo.RelationshipType");
            DropForeignKey("dbo.RoleType", "RelationshipType_Id", "dbo.RelationshipType");
            DropForeignKey("dbo.PartyRole", "PartyRelationship_Id", "dbo.PartyRelationship");
            DropForeignKey("dbo.WorkEffortRate", "EmployeeRole_Id", "dbo.PartyRole");
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
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.Organization", new[] { "Id" });
            DropIndex("dbo.Person", new[] { "Id" });
            DropIndex("dbo.WorkEffortPartyAssignment", new[] { "WorkEffort_Id" });
            DropIndex("dbo.WorkEffortPartyAssignment", new[] { "AssignedTo_Id" });
            DropIndex("dbo.WorkEffortHistorycalRecord", new[] { "WorkEffortPartyAssignment_Id" });
            DropIndex("dbo.WorkEffortAssociation", new[] { "WorkEffort_Id" });
            DropIndex("dbo.WorkEffortAssociation", new[] { "TypeOfAssociatedWorkEffort_Id" });
            DropIndex("dbo.WorkEffort", new[] { "Type_Id" });
            DropIndex("dbo.WorkEffort", new[] { "RequerdRole_Id" });
            DropIndex("dbo.WorkEffort", new[] { "AssignedToParty_Id" });
            DropIndex("dbo.RelationshipConstraine", new[] { "RuleSet_Id" });
            DropIndex("dbo.RelationshipConstraine", new[] { "Role_Id" });
            DropIndex("dbo.RuleSet", new[] { "Role_Id" });
            DropIndex("dbo.RuleSet", new[] { "Relationship_Id" });
            DropIndex("dbo.WorkEffortRate", new[] { "EmployeeRole_Id" });
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
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "Party_Id" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropTable("dbo.Organization");
            DropTable("dbo.Person");
            DropTable("dbo.WorkEffortPartyAssignment");
            DropTable("dbo.WorkEffortHistorycalRecord");
            DropTable("dbo.WorkEffortAssociation");
            DropTable("dbo.WorkEffortType");
            DropTable("dbo.WorkEffort");
            DropTable("dbo.RelationshipConstraine");
            DropTable("dbo.RuleSet");
            DropTable("dbo.RelationshipType");
            DropTable("dbo.WorkEffortRate");
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
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
        }
    }
}
