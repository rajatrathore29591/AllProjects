namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attachments",
                c => new
                    {
                        AttachmentId = c.Guid(nullable: false),
                        AttachmentTitle = c.String(maxLength: 30),
                        IsAttachment = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        EditedBy = c.Guid(),
                        CreatedDate = c.DateTime(nullable: false),
                        EditedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.AttachmentId);
            
            CreateTable(
                "dbo.BookingFleets",
                c => new
                    {
                        BookingFleetId = c.Guid(nullable: false),
                        BookingId = c.Guid(nullable: false),
                        FleetTypeId = c.Guid(nullable: false),
                        FleetRegistrationId = c.Guid(),
                        DriverId = c.Guid(),
                        IsDayShift = c.Boolean(nullable: false),
                        Iswethire = c.Boolean(nullable: false),
                        AttachmentIds = c.String(maxLength: 4000),
                        NotesForDrive = c.String(maxLength: 4000),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        EditedBy = c.Guid(),
                        CreatedDate = c.DateTime(nullable: false),
                        EditedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.BookingFleetId)
                .ForeignKey("dbo.Booking", t => t.BookingId, cascadeDelete: true)
                .ForeignKey("dbo.Drivers", t => t.DriverId)
                .ForeignKey("dbo.FleetsRegistration", t => t.FleetRegistrationId)
                .ForeignKey("dbo.FleetTypes", t => t.FleetTypeId, cascadeDelete: true)
                .Index(t => t.BookingId)
                .Index(t => t.FleetTypeId)
                .Index(t => t.FleetRegistrationId)
                .Index(t => t.DriverId);
            
            CreateTable(
                "dbo.Booking",
                c => new
                    {
                        BookingId = c.Guid(nullable: false),
                        CustomerId = c.Guid(nullable: false),
                        SiteId = c.Guid(),
                        WorktypeId = c.Guid(nullable: false),
                        StatusLookupId = c.Guid(nullable: false),
                        CallingDateTime = c.DateTime(nullable: false),
                        FleetBookingDateTime = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        EditedBy = c.Guid(),
                        CreatedDate = c.DateTime(nullable: false),
                        EditedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.BookingId)
                .ForeignKey("dbo.Customer", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.Sites", t => t.SiteId)
                .ForeignKey("dbo.StatusLookup", t => t.StatusLookupId, cascadeDelete: true)
                .ForeignKey("dbo.WorkTypes", t => t.WorktypeId, cascadeDelete: true)
                .Index(t => t.CustomerId)
                .Index(t => t.SiteId)
                .Index(t => t.WorktypeId)
                .Index(t => t.StatusLookupId);
            
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        CustomerId = c.Guid(nullable: false),
                        CustomerName = c.String(nullable: false, maxLength: 100),
                        ABN = c.String(maxLength: 50),
                        OfficeStreet = c.String(maxLength: 100),
                        OfficeStreetPoBox = c.String(maxLength: 20),
                        OfficeSuburb = c.String(maxLength: 50),
                        PostalSuburb = c.String(maxLength: 50),
                        OfficePostalCode = c.String(maxLength: 10),
                        PostalState = c.String(maxLength: 10),
                        PostalPostCode = c.String(maxLength: 10),
                        PhoneNumber1 = c.String(maxLength: 12),
                        PhoneNumber2 = c.String(maxLength: 12),
                        MobileNumber1 = c.String(maxLength: 12),
                        MobileNumber2 = c.String(maxLength: 12),
                        Fax = c.String(maxLength: 12),
                        ContactName = c.String(maxLength: 80),
                        ContactNumber = c.String(maxLength: 12),
                        AccountsContact = c.String(maxLength: 50),
                        AccountsNumber = c.String(maxLength: 50),
                        EmailForInvoices = c.String(maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        EditedBy = c.Guid(),
                        CreatedDate = c.DateTime(nullable: false),
                        EditedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CustomerId);
            
            CreateTable(
                "dbo.Sites",
                c => new
                    {
                        SiteId = c.Guid(nullable: false),
                        CustomerId = c.Guid(nullable: false),
                        SiteName = c.String(nullable: false, maxLength: 100),
                        SiteDetail = c.String(maxLength: 4000),
                        DayShiftRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NightShiftRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FloatCharge = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PilotCharge = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TipOffRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FuelIncluded = c.Boolean(nullable: false),
                        TollTax = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        EditedBy = c.Guid(),
                        CreatedDate = c.DateTime(nullable: false),
                        EditedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SiteId)
                .ForeignKey("dbo.Customer", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Gate",
                c => new
                    {
                        GateId = c.Guid(nullable: false),
                        SiteId = c.Guid(nullable: false),
                        GateNumber = c.String(maxLength: 20),
                        ContactPerson = c.String(maxLength: 100),
                        Email = c.String(maxLength: 100),
                        MobileNumber = c.String(maxLength: 12),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        EditedBy = c.Guid(),
                        CreatedDate = c.DateTime(nullable: false),
                        EditedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.GateId)
                .ForeignKey("dbo.Sites", t => t.SiteId, cascadeDelete: true)
                .Index(t => t.SiteId);
            
            CreateTable(
                "dbo.BookingSiteGates",
                c => new
                    {
                        BookingSiteGateId = c.Guid(nullable: false),
                        GateId = c.Guid(nullable: false),
                        Note = c.String(maxLength: 4000),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        EditedBy = c.Guid(),
                        CreatedDate = c.DateTime(nullable: false),
                        EditedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.BookingSiteGateId)
                .ForeignKey("dbo.Gate", t => t.GateId, cascadeDelete: true)
                .Index(t => t.GateId);
            
            CreateTable(
                "dbo.Supervisor",
                c => new
                    {
                        SupervisorId = c.Guid(nullable: false),
                        SiteId = c.Guid(nullable: false),
                        SupervisorName = c.String(nullable: false, maxLength: 100),
                        Email = c.String(maxLength: 100),
                        MobileNumber = c.String(maxLength: 12),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        EditedBy = c.Guid(),
                        CreatedDate = c.DateTime(nullable: false),
                        EditedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SupervisorId)
                .ForeignKey("dbo.Sites", t => t.SiteId, cascadeDelete: true)
                .Index(t => t.SiteId);
            
            CreateTable(
                "dbo.BookingSiteSupervisor",
                c => new
                    {
                        BookingSiteSupervisorId = c.Guid(nullable: false),
                        SupervisorId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        EditedBy = c.Guid(),
                        CreatedDate = c.DateTime(nullable: false),
                        EditedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.BookingSiteSupervisorId)
                .ForeignKey("dbo.Supervisor", t => t.SupervisorId, cascadeDelete: true)
                .Index(t => t.SupervisorId);
            
            CreateTable(
                "dbo.StatusLookup",
                c => new
                    {
                        StatusLookupId = c.Guid(nullable: false),
                        Title = c.String(maxLength: 20),
                        Group = c.String(maxLength: 20),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        EditedBy = c.Guid(),
                        CreatedDate = c.DateTime(nullable: false),
                        EditedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.StatusLookupId);
            
            CreateTable(
                "dbo.WorkTypes",
                c => new
                    {
                        WorkTypeId = c.Guid(nullable: false),
                        Type = c.String(maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        EditedBy = c.Guid(),
                        CreatedDate = c.DateTime(nullable: false),
                        EditedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.WorkTypeId);
            
            CreateTable(
                "dbo.Drivers",
                c => new
                    {
                        DriverId = c.Guid(nullable: false),
                        FleetRegistrationId = c.Guid(),
                        CountryId = c.Guid(nullable: false),
                        EmploymentCategoryId = c.Guid(nullable: false),
                        ProfilePic = c.Guid(),
                        LicenseClassId = c.Guid(),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(maxLength: 50),
                        Email = c.String(maxLength: 100),
                        MobileNumber = c.String(nullable: false, maxLength: 12),
                        AddressLine1 = c.String(maxLength: 4000),
                        AddressLine2 = c.String(maxLength: 4000),
                        Street = c.String(maxLength: 30),
                        Suburb = c.String(maxLength: 50),
                        State = c.String(maxLength: 30),
                        PostCode = c.String(maxLength: 10),
                        CardNumber = c.String(maxLength: 50),
                        DocumentIds = c.String(maxLength: 300),
                        LicenseNumber = c.String(maxLength: 30),
                        ExpiryDate = c.DateTime(),
                        BaseRate = c.Decimal(precision: 18, scale: 2),
                        Shift = c.String(maxLength: 10),
                        Awards = c.String(maxLength: 4000),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        EditedBy = c.Guid(),
                        CreatedDate = c.DateTime(nullable: false),
                        EditedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DriverId)
                .ForeignKey("dbo.FleetsRegistration", t => t.FleetRegistrationId)
                .Index(t => t.FleetRegistrationId);
            
            CreateTable(
                "dbo.DriversFleetsMapping",
                c => new
                    {
                        DriverFleetMapping = c.Guid(nullable: false),
                        DriverId = c.Guid(nullable: false),
                        FleetRegistrationId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        EditedBy = c.Guid(),
                        CreatedDate = c.DateTime(nullable: false),
                        EditedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DriverFleetMapping)
                .ForeignKey("dbo.Drivers", t => t.DriverId, cascadeDelete: true)
                .ForeignKey("dbo.FleetsRegistration", t => t.FleetRegistrationId, cascadeDelete: true)
                .Index(t => t.DriverId)
                .Index(t => t.FleetRegistrationId);
            
            CreateTable(
                "dbo.FleetsRegistration",
                c => new
                    {
                        FleetRegistrationId = c.Guid(nullable: false),
                        FleetTypeId = c.Guid(nullable: false),
                        Make = c.String(maxLength: 50),
                        Model = c.String(maxLength: 50),
                        Capacity = c.String(maxLength: 10),
                        Year = c.String(maxLength: 5),
                        Registration = c.String(maxLength: 10),
                        BorgCivilPlantNumber = c.String(maxLength: 10),
                        VINNumber = c.String(maxLength: 50),
                        EngineNumber = c.String(maxLength: 50),
                        InsuranceDate = c.DateTime(nullable: false),
                        CurrentMeterReading = c.String(maxLength: 50),
                        LastServiceMeterReading = c.String(maxLength: 30),
                        ServiceInterval = c.String(maxLength: 30),
                        HVISType = c.String(maxLength: 30),
                        IsBooked = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        EditedBy = c.Guid(),
                        CreatedDate = c.DateTime(nullable: false),
                        EditedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.FleetRegistrationId)
                .ForeignKey("dbo.FleetTypes", t => t.FleetTypeId, cascadeDelete: true)
                .Index(t => t.FleetTypeId);
            
            CreateTable(
                "dbo.FleetTypes",
                c => new
                    {
                        FleetTypeId = c.Guid(nullable: false),
                        Fleet = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 4000),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        EditedBy = c.Guid(),
                        CreatedDate = c.DateTime(nullable: false),
                        EditedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.FleetTypeId);
            
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        DocumentId = c.Guid(nullable: false),
                        Name = c.String(maxLength: 100),
                        OriginalName = c.String(maxLength: 100),
                        URL = c.String(maxLength: 255),
                        Title = c.String(maxLength: 100),
                        Description = c.String(maxLength: 4000),
                        Extension = c.String(maxLength: 20),
                        FileSize = c.Int(nullable: false),
                        Private = c.Boolean(nullable: false),
                        Tags = c.String(maxLength: 4000),
                        ThumbnailFileName = c.String(maxLength: 100),
                        CreatedDate = c.DateTime(nullable: false),
                        EditedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DocumentId);
            
            CreateTable(
                "dbo.Fleets",
                c => new
                    {
                        FleetId = c.Guid(nullable: false),
                        Fleet = c.String(maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        EditedBy = c.Guid(),
                        CreatedDate = c.DateTime(nullable: false),
                        EditedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.FleetId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Description = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
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
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.BookingFleets", "FleetTypeId", "dbo.FleetTypes");
            DropForeignKey("dbo.BookingFleets", "FleetRegistrationId", "dbo.FleetsRegistration");
            DropForeignKey("dbo.BookingFleets", "DriverId", "dbo.Drivers");
            DropForeignKey("dbo.Drivers", "FleetRegistrationId", "dbo.FleetsRegistration");
            DropForeignKey("dbo.DriversFleetsMapping", "FleetRegistrationId", "dbo.FleetsRegistration");
            DropForeignKey("dbo.FleetsRegistration", "FleetTypeId", "dbo.FleetTypes");
            DropForeignKey("dbo.DriversFleetsMapping", "DriverId", "dbo.Drivers");
            DropForeignKey("dbo.BookingFleets", "BookingId", "dbo.Booking");
            DropForeignKey("dbo.Booking", "WorktypeId", "dbo.WorkTypes");
            DropForeignKey("dbo.Booking", "StatusLookupId", "dbo.StatusLookup");
            DropForeignKey("dbo.Booking", "SiteId", "dbo.Sites");
            DropForeignKey("dbo.Booking", "CustomerId", "dbo.Customer");
            DropForeignKey("dbo.Supervisor", "SiteId", "dbo.Sites");
            DropForeignKey("dbo.BookingSiteSupervisor", "SupervisorId", "dbo.Supervisor");
            DropForeignKey("dbo.Gate", "SiteId", "dbo.Sites");
            DropForeignKey("dbo.BookingSiteGates", "GateId", "dbo.Gate");
            DropForeignKey("dbo.Sites", "CustomerId", "dbo.Customer");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.FleetsRegistration", new[] { "FleetTypeId" });
            DropIndex("dbo.DriversFleetsMapping", new[] { "FleetRegistrationId" });
            DropIndex("dbo.DriversFleetsMapping", new[] { "DriverId" });
            DropIndex("dbo.Drivers", new[] { "FleetRegistrationId" });
            DropIndex("dbo.BookingSiteSupervisor", new[] { "SupervisorId" });
            DropIndex("dbo.Supervisor", new[] { "SiteId" });
            DropIndex("dbo.BookingSiteGates", new[] { "GateId" });
            DropIndex("dbo.Gate", new[] { "SiteId" });
            DropIndex("dbo.Sites", new[] { "CustomerId" });
            DropIndex("dbo.Booking", new[] { "StatusLookupId" });
            DropIndex("dbo.Booking", new[] { "WorktypeId" });
            DropIndex("dbo.Booking", new[] { "SiteId" });
            DropIndex("dbo.Booking", new[] { "CustomerId" });
            DropIndex("dbo.BookingFleets", new[] { "DriverId" });
            DropIndex("dbo.BookingFleets", new[] { "FleetRegistrationId" });
            DropIndex("dbo.BookingFleets", new[] { "FleetTypeId" });
            DropIndex("dbo.BookingFleets", new[] { "BookingId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Fleets");
            DropTable("dbo.Documents");
            DropTable("dbo.FleetTypes");
            DropTable("dbo.FleetsRegistration");
            DropTable("dbo.DriversFleetsMapping");
            DropTable("dbo.Drivers");
            DropTable("dbo.WorkTypes");
            DropTable("dbo.StatusLookup");
            DropTable("dbo.BookingSiteSupervisor");
            DropTable("dbo.Supervisor");
            DropTable("dbo.BookingSiteGates");
            DropTable("dbo.Gate");
            DropTable("dbo.Sites");
            DropTable("dbo.Customer");
            DropTable("dbo.Booking");
            DropTable("dbo.BookingFleets");
            DropTable("dbo.Attachments");
        }
    }
}
