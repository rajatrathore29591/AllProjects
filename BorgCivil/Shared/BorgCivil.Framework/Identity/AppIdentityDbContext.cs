using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using BorgCivil.Framework.Entities;

namespace BorgCivil.Framework.Identity
{

    public partial class AppIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppIdentityDbContext()
            : base("BorgCivil.ContextConnection")
        {

        }

        #region Set of Entity

        //creating DBset for each table
        public virtual DbSet<Attachment> Attachments { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<BookingFleet> BookingFleet { get; set; }
        public virtual DbSet<BookingSiteGate> BookingSiteGates { get; set; }
        public virtual DbSet<BookingSiteSupervisor> BookingSiteSupervisor { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Country> Countrys { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Document> Document { get; set; }
        public virtual DbSet<Docket> Dockets { get; set; }
        public virtual DbSet<DocketCheckList> DocketCheckLists { get; set; }
        public virtual DbSet<Driver> Drivers { get; set; }
        public virtual DbSet<DriversFleetsMapping> DriversFleetsMappings { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmailSetting> EmailSetting { get; set; }
        public virtual DbSet<EmailTemplate> EmailTemplate { get; set; }
        public virtual DbSet<Fleets> Fleets { get; set; }
        public virtual DbSet<FleetHistory> FleetHistory { get; set; }
        public virtual DbSet<FleetsRegistration> FleetsRegistrations { get; set; }
        public virtual DbSet<Gate> Gates { get; set; }
        public virtual DbSet<LoadDocket> LoadDockets { get; set; }
        public virtual DbSet<Site> Sites { get; set; }
        public virtual DbSet<StatusLookup> StatusLookups { get; set; }
        public virtual DbSet<Supervisor> Supervisors { get; set; }
        public virtual DbSet<WorkType> WorkTypes { get; set; }
        public virtual DbSet<EmploymentCategory> EmploymentCategorys { get; set; }
        public virtual DbSet<LicenseClass> LicenseClasss { get; set; }
        public virtual DbSet<Demo> Demos { get; set; }
        public virtual DbSet<SentEmail> SentEmail { get; set; }
        public virtual DbSet<State> States { get; set; }

        #endregion

        public static AppIdentityDbContext Create()
        {
            return new AppIdentityDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            #region  Foreign Key Rules

            //AnonymousField
            modelBuilder.Entity<AnonymousField>()
                .HasOptional(pk => pk.Driver)
                .WithMany(cl => cl.AnonymousFields)
                .HasForeignKey(fk => fk.DriverId)
                .WillCascadeOnDelete(true);

            //Attachments
            modelBuilder.Entity<Attachment>()
                .HasOptional(pk => pk.Companies)
                .WithMany(cl => cl.Attachments)
                .HasForeignKey(fk => fk.CompanyId);

            //Customer
            modelBuilder.Entity<Customer>()
                .HasOptional(pk => pk.Companies)
                .WithMany(cl => cl.Customers)
                .HasForeignKey(fk => fk.CompanyId);

            //Booking
            modelBuilder.Entity<Booking>()
                .HasRequired(pk => pk.Customer)
                .WithMany(cl => cl.Bookings)
                .HasForeignKey(fk => fk.CustomerId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Booking>()
               .HasOptional(pk => pk.Site)
               .WithMany(cl => cl.Bookings)
               .HasForeignKey(fk => fk.SiteId)
               .WillCascadeOnDelete(true);

            modelBuilder.Entity<Booking>()
                .HasRequired(pk => pk.WorkType)
                .WithMany(cl => cl.Bookings)
                .HasForeignKey(fk => fk.WorktypeId)
                 .WillCascadeOnDelete(false);

            modelBuilder.Entity<Booking>()
                .HasRequired(pk => pk.StatusLookup)
                .WithMany(cl => cl.Bookings)
                .HasForeignKey(fk => fk.StatusLookupId)
                 .WillCascadeOnDelete(false);

            ////BookingFleets
            modelBuilder.Entity<BookingFleet>()
                .HasRequired(pk => pk.Booking)
                .WithMany(cl => cl.BookingFleets)
                .HasForeignKey(fk => fk.BookingId);

            modelBuilder.Entity<BookingFleet>()
              .HasRequired(pk => pk.FleetType)
              .WithMany(cl => cl.BookingFleets)
              .HasForeignKey(fk => fk.FleetTypeId);

            modelBuilder.Entity<BookingFleet>()
                .HasOptional(pk => pk.FleetsRegistration)
                .WithMany(cl => cl.BookingFleets)
                .HasForeignKey(fk => fk.FleetRegistrationId);

            modelBuilder.Entity<BookingFleet>()
                .HasOptional(pk => pk.Driver)
                .WithMany(cl => cl.BookingFleets)
                .HasForeignKey(fk => fk.DriverId);

            modelBuilder.Entity<BookingFleet>()
              .HasOptional(pk => pk.StatusLookup)
              .WithMany(cl => cl.BookingFleets)
              .HasForeignKey(fk => fk.StatusLookupId);

            ////BookingSiteGates
            modelBuilder.Entity<BookingSiteGate>()
                .HasRequired(pk => pk.Gate)
                .WithMany(cl => cl.BookingSiteGates)
                .HasForeignKey(fk => fk.GateId);

            modelBuilder.Entity<BookingSiteGate>()
                 .HasRequired(pk => pk.FleetsRegistration)
                 .WithMany(cl => cl.BookingSiteGates)
                 .HasForeignKey(fk => fk.FleetRegistrationId);

            modelBuilder.Entity<BookingSiteGate>()
                  .HasRequired(pk => pk.Booking)
                  .WithMany(cl => cl.BookingSiteGates)
                  .HasForeignKey(fk => fk.BookingId)
                  .WillCascadeOnDelete(false);

            modelBuilder.Entity<BookingSiteGate>()
                .HasRequired(pk => pk.GateContactPerson)
                .WithMany(cl => cl.BookingSiteGates)
                .HasForeignKey(fk => fk.GateContactPersonId)
                .WillCascadeOnDelete(false);

            ////BookingSiteSupervisor
            modelBuilder.Entity<BookingSiteSupervisor>()
                .HasRequired(pk => pk.Supervisor)
                .WithMany(cl => cl.BookingSiteSupervisors)
                .HasForeignKey(fk => fk.SupervisorId);

            modelBuilder.Entity<BookingSiteSupervisor>()
               .HasRequired(pk => pk.Booking)
               .WithMany(cl => cl.BookingSiteSupervisors)
               .HasForeignKey(fk => fk.BookingId)
               .WillCascadeOnDelete(false); ;

            ////Driver
            modelBuilder.Entity<Driver>()
                .HasOptional(pk => pk.FleetsRegistration)
                .WithMany(cl => cl.Drivers)
                .HasForeignKey(fk => fk.FleetRegistrationId);

            modelBuilder.Entity<Driver>()
              .HasRequired(pk => pk.Country)
              .WithMany(cl => cl.Drivers)
              .HasForeignKey(fk => fk.CountryId);

            modelBuilder.Entity<Driver>()
              .HasOptional(pk => pk.State)
              .WithMany(cl => cl.Drivers)
              .HasForeignKey(fk => fk.StateId);

            modelBuilder.Entity<Driver>()
              .HasOptional(pk => pk.EmploymentCategory)
              .WithMany(cl => cl.Drivers)
              .HasForeignKey(fk => fk.EmploymentCategoryId);

            modelBuilder.Entity<Driver>()
             .HasOptional(pk => pk.LicenseClass)
             .WithMany(cl => cl.Drivers)
             .HasForeignKey(fk => fk.LicenseClassId);

            modelBuilder.Entity<Driver>()
                .HasOptional(pk => pk.Document)
                .WithMany(cl => cl.Drivers)
                .HasForeignKey(fk => fk.ProfilePic);

            modelBuilder.Entity<Driver>()
               .HasOptional(pk => pk.StatusLookup)
               .WithMany(cl => cl.Drivers)
               .HasForeignKey(fk => fk.StatusLookupId);

            ////DriverWhiteCard
            modelBuilder.Entity<DriverWhiteCard>()
                .HasOptional(pk => pk.Drivers)
                .WithMany(cl => cl.DriverWhiteCards)
                .HasForeignKey(fk => fk.DriverId)
                .WillCascadeOnDelete(true);

            ////DriverInductionCard
            modelBuilder.Entity<DriverInductionCard>()
                .HasOptional(pk => pk.Drivers)
                .WithMany(cl => cl.DriverInductionCards)
                .HasForeignKey(fk => fk.DriverId)
                .WillCascadeOnDelete(true);

            ////DriverVocCard
            modelBuilder.Entity<DriverVocCard>()
                .HasOptional(pk => pk.Drivers)
                .WithMany(cl => cl.DriverVocCards)
                .HasForeignKey(fk => fk.DriverId)
                .WillCascadeOnDelete(true);

            ////DriversFleetMapping
            modelBuilder.Entity<DriversFleetsMapping>()
                .HasRequired(pk => pk.Driver)
                .WithMany(cl => cl.DriversFleetsMappings)
                .HasForeignKey(fk => fk.DriverId);

            modelBuilder.Entity<DriversFleetsMapping>()
                .HasRequired(pk => pk.FleetRegistration)
                .WithMany(cl => cl.DriversFleetsMappings)
                .HasForeignKey(fk => fk.FleetRegistrationId);

            ////Docket
            modelBuilder.Entity<Docket>()
                .HasOptional(pk => pk.BookingFleet)
                .WithMany(cl => cl.Dockets)
                .HasForeignKey(fk => fk.BookingFleetId);

            modelBuilder.Entity<Docket>()
                .HasOptional(pk => pk.FleetsRegistration)
                .WithMany(cl => cl.Dockets)
                .HasForeignKey(fk => fk.FleetRegistrationId);

            modelBuilder.Entity<Docket>()
               .HasOptional(pk => pk.Supervisor)
               .WithMany(cl => cl.Dockets)
               .HasForeignKey(fk => fk.SupervisorId);

            modelBuilder.Entity<Docket>()
                 .HasOptional(pk => pk.Document)
                 .WithMany(cl => cl.Dockets)
                 .HasForeignKey(fk => fk.DocumentId);

            ////Employee
            modelBuilder.Entity<Employee>()
                .HasOptional(pk => pk.EmploymentCategory)
                .WithMany(cl => cl.Employees)
                .HasForeignKey(fk => fk.EmploymentCategoryId);

            modelBuilder.Entity<Employee>()
                .HasOptional(pk => pk.StatusLookup)
                .WithMany(cl => cl.Employees)
                .HasForeignKey(fk => fk.EmploymentStatusId);

            modelBuilder.Entity<Employee>()
             .HasRequired(pk => pk.User)
             .WithMany(cl => cl.Employees)
             .HasForeignKey(fk => fk.UserId);

            modelBuilder.Entity<Employee>()
               .HasOptional(pk => pk.Country)
               .WithMany(cl => cl.Employees)
               .HasForeignKey(fk => fk.CountryId);

            modelBuilder.Entity<Employee>()
               .HasOptional(pk => pk.State)
               .WithMany(cl => cl.Employees)
               .HasForeignKey(fk => fk.StateId);

            modelBuilder.Entity<Employee>()
              .HasOptional(pk => pk.Role)
              .WithMany(cl => cl.Employees)
              .HasForeignKey(fk => fk.RoleId);

            modelBuilder.Entity<Employee>()
             .HasOptional(pk => pk.Document)
             .WithMany(cl => cl.Employees)
             .HasForeignKey(fk => fk.DocumentId);

            ////EmployementCategory
            modelBuilder.Entity<EmploymentCategory>()
                .HasOptional(pk => pk.Companies)
                .WithMany(cl => cl.EmploymentCategorys)
                .HasForeignKey(fk => fk.CompanyId);

            ////FleetHistory
            modelBuilder.Entity<FleetHistory>()
                .HasOptional(pk => pk.BookingFleet)
                .WithMany(cl => cl.FleetHistorys)
                .HasForeignKey(fk => fk.BookingFleetId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<FleetHistory>()
               .HasOptional(pk => pk.Booking)
               .WithMany(cl => cl.FleetHistorys)
               .HasForeignKey(fk => fk.BookingId);

            modelBuilder.Entity<FleetHistory>()
               .HasOptional(pk => pk.FleetType)
               .WithMany(cl => cl.FleetHistorys)
               .HasForeignKey(fk => fk.FleetTypeId);


            modelBuilder.Entity<FleetHistory>()
               .HasOptional(pk => pk.FleetsRegistration)
               .WithMany(cl => cl.FleetHistorys)
               .HasForeignKey(fk => fk.FleetRegistrationId);

            modelBuilder.Entity<FleetHistory>()
              .HasOptional(pk => pk.Driver)
              .WithMany(cl => cl.FleetHistorys)
              .HasForeignKey(fk => fk.DriverId);

            modelBuilder.Entity<FleetHistory>()
              .HasOptional(pk => pk.StatusLookup)
              .WithMany(cl => cl.FleetHistorys)
              .HasForeignKey(fk => fk.FleetStatus);

            ////FleetTypes
            modelBuilder.Entity<FleetType>()
                .HasOptional(pk => pk.Companies)
                .WithMany(cl => cl.FleetTypes)
                .HasForeignKey(fk => fk.CompanyId);

            modelBuilder.Entity<FleetType>()
               .HasOptional(pk => pk.Documents)
               .WithMany(cl => cl.FleetTypes)
               .HasForeignKey(fk => fk.DocumentId);

            ////FleetRegistration
            modelBuilder.Entity<FleetsRegistration>()
                .HasRequired(pk => pk.FleetType)
                .WithMany(cl => cl.FleetsRegistrations)
                .HasForeignKey(fk => fk.FleetTypeId);

            modelBuilder.Entity<FleetsRegistration>()
               .HasOptional(pk => pk.Attachment)
               .WithMany(cl => cl.FleetsRegistrations)
               .HasForeignKey(fk => fk.AttachmentId);

            modelBuilder.Entity<FleetsRegistration>()
              .HasOptional(pk => pk.Subcontractor)
              .WithMany(cl => cl.FleetsRegistrations)
              .HasForeignKey(fk => fk.SubcontractorId);

            ////Gate
            modelBuilder.Entity<Gate>()
                .HasRequired(pk => pk.Site)
                .WithMany(cl => cl.Gates)
                .HasForeignKey(fk => fk.SiteId);

            ////GateContactPerson
            modelBuilder.Entity<GateContactPerson>()
                .HasRequired(pk => pk.Gate)
                .WithMany(cl => cl.GateContactPersons)
                .HasForeignKey(fk => fk.GateId);

            ////LicenseClass
            modelBuilder.Entity<LicenseClass>()
                .HasOptional(pk => pk.Companies)
                .WithMany(cl => cl.LicenseClass)
                .HasForeignKey(fk => fk.CompanyId);

            ////LoadDocket
            modelBuilder.Entity<LoadDocket>()
                .HasOptional(pk => pk.Docket)
                .WithMany(cl => cl.LoadDockets)
                .HasForeignKey(fk => fk.DocketId);

            ////Site
            modelBuilder.Entity<Site>()
                .HasRequired(pk => pk.Customer)
                .WithMany(cl => cl.Sites)
                .HasForeignKey(fk => fk.CustomerId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Site>()
                .HasOptional(pk => pk.Document)
                .WithMany(cl => cl.Sites)
                .HasForeignKey(fk => fk.DocumentId);

            ////StatusLookUp
            modelBuilder.Entity<StatusLookup>()
                .HasOptional(pk => pk.Companies)
                .WithMany(cl => cl.StatusLookups)
                .HasForeignKey(fk => fk.CompanyId);

            ////Supervisor
            modelBuilder.Entity<Supervisor>()
                .HasRequired(pk => pk.Site)
                .WithMany(cl => cl.Supervisors)
                .HasForeignKey(fk => fk.SiteId);

            ////State
            modelBuilder.Entity<State>()
                .HasOptional(pk => pk.Country)
                .WithMany(cl => cl.States)
                .HasForeignKey(fk => fk.CountryId);

            #endregion

            #region Data Type Rules
            // creating rule for decimal type field
            modelBuilder.Entity<Site>().Property(x => x.DayShiftRate).HasPrecision(18, 2);
            modelBuilder.Entity<Site>().Property(x => x.NightShiftRate).HasPrecision(18, 2);
            modelBuilder.Entity<Site>().Property(x => x.FloatCharge).HasPrecision(18, 2);
            modelBuilder.Entity<Site>().Property(x => x.PilotCharge).HasPrecision(18, 2);
            modelBuilder.Entity<Site>().Property(x => x.TipOffRate).HasPrecision(18, 2);
            modelBuilder.Entity<Driver>().Property(x => x.BaseRate).HasPrecision(18, 2);
            modelBuilder.Entity<Booking>().Property(x => x.Rate).HasPrecision(18, 2);

            #endregion

            base.OnModelCreating(modelBuilder);

        }
    }
}
