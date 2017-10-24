using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using TradeSystem.Framework.Entities;

namespace TradeSystem.Framework.Identity
{

    public partial class AppIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppIdentityDbContext()
            : base("TradeSystem.ContextConnection")
        {

        }

        #region Set of Entity

        //For App
        public virtual DbSet<Bank> Banks { get; set; }
        public virtual DbSet<CompanyUser> CompanyUsers { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerProduct> CustomerProducts { get; set; }
        public virtual DbSet<CustomerTransaction> CustomerTransactions { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<RoleSideMenu> RoleSideMenus { get; set; }
        public virtual DbSet<SideMenu> SideMenus { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }  
        public virtual DbSet<TicketStatus> TicketStatus { get; set; }
        public virtual DbSet<Withdraw> Withdraws { get; set; }
        public virtual DbSet<EmailSetting> EmailSettings { get; set; }
        public virtual DbSet<EmailTemplate> EmailTemplates { get; set; }
        public virtual DbSet<Penalty> Penaltys { get; set; }
        public virtual DbSet<AdminConfig> AdminConfigs { get; set; }
        public virtual DbSet<Country> Countrys { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<SentEmail> SentEmails { get; set; }
        public virtual DbSet<Promotion> Promotions { get; set; }
        public virtual DbSet<ActivityLog> ActivityLogs { get; set; }

        #endregion

        public static AppIdentityDbContext Create()
        {
            return new AppIdentityDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            #region  Foreign Key Rules
            //CompanyUser
            modelBuilder.Entity<CompanyUser>()
                .HasOptional(pk => pk.Document)
                .WithMany(cl => cl.CompanyUsers)
                .HasForeignKey(fk => fk.DocumentId);

            modelBuilder.Entity<CompanyUser>()
               .HasRequired(pk => pk.User)
               .WithMany(cl => cl.CompanyUsers)
               .HasForeignKey(fk => fk.UserId);
             

            modelBuilder.Entity<CompanyUser>()
                .HasRequired(pk => pk.Role)
                .WithMany(cl => cl.CompanyUsers)
                .HasForeignKey(fk => fk.RoleId);

            //Customer
            modelBuilder.Entity<Customer>()
                .HasOptional(pk => pk.Document)
                .WithMany(cl => cl.Customers)
                .HasForeignKey(fk => fk.DocumentId);

            modelBuilder.Entity<Customer>()
              .HasRequired(pk => pk.User)
              .WithMany(cl => cl.Customers)
              .HasForeignKey(fk => fk.UserId);

            modelBuilder.Entity<Customer>()
                .HasOptional(pk => pk.Customers)
                .WithMany(cl => cl.Customerss)
                .HasForeignKey(fk => fk.CustomerReferalId);

            modelBuilder.Entity<Customer>()
                .HasOptional(pk => pk.Bank)
                .WithMany(cl => cl.Customers)
                .HasForeignKey(fk => fk.BankId);

            modelBuilder.Entity<Customer>()
              .HasOptional(pk => pk.Country)
              .WithMany(cl => cl.Customers)
              .HasForeignKey(fk => fk.CountryId);

            modelBuilder.Entity<Customer>()
                .HasOptional(pk => pk.State)
                .WithMany(cl => cl.Customers)
                .HasForeignKey(fk => fk.StateId);

            //CustomerProduct
            modelBuilder.Entity<CustomerProduct>()
                .HasRequired(pk => pk.Customer)
                .WithMany(cl => cl.CustomerProducts)
                .HasForeignKey(fk => fk.CustomerId);

            modelBuilder.Entity<CustomerProduct>()
                .HasRequired(pk => pk.Product)
                .WithMany(cl => cl.CustomerProducts)
                .HasForeignKey(fk => fk.ProductId);

            //CustomerTransaction
            modelBuilder.Entity<CustomerTransaction>()
                .HasRequired(pk => pk.CustomerProduct)
                .WithMany(cl => cl.CustomerTransactions)
                .HasForeignKey(fk => fk.CustomerProductId);

            //Product
            modelBuilder.Entity<Product>()
                .HasOptional(pk => pk.Document)
                .WithMany(cl => cl.Products)
                .HasForeignKey(fk => fk.DocumentId);

            //RoleSideMenu
            modelBuilder.Entity<RoleSideMenu>()
                .HasRequired(pk => pk.SideMenu)
                .WithMany(cl => cl.RoleSideMenus)
                .HasForeignKey(fk => fk.SideMenuId);

            modelBuilder.Entity<RoleSideMenu>()
                .HasRequired(pk => pk.Role)
                .WithMany(cl => cl.RoleSideMenus)
                .HasForeignKey(fk => fk.RoleId);

            //Ticket
            modelBuilder.Entity<Ticket>()
                .HasRequired(pk => pk.TicketStatus)
                .WithMany(cl => cl.Tickets)
                .HasForeignKey(fk => fk.TicketStatusId);

            modelBuilder.Entity<Ticket>()
                .HasRequired(pk => pk.Customer)
                .WithMany(cl => cl.Tickets)
                .HasForeignKey(fk => fk.CustomerId);

            //Withdraw
            modelBuilder.Entity<Withdraw>()
                .HasRequired(pk => pk.CustomerProduct)
                .WithMany(cl => cl.Withdraws)
                .HasForeignKey(fk => fk.CustomerProductId);

            //State
            modelBuilder.Entity<State>()
                .HasRequired(pk => pk.Country)
                .WithMany(cl => cl.States)
                .HasForeignKey(fk => fk.CountryId);

            //Penalty
            modelBuilder.Entity<Penalty>()
                .HasRequired(pk => pk.Product)
                .WithMany(cl => cl.Penaltys)
                .HasForeignKey(fk => fk.ProductId);

            //ActivityLog
            modelBuilder.Entity<ActivityLog>()
                .HasRequired(pk => pk.CompanyUser)
                .WithMany(cl => cl.ActivityLogs)
                .HasForeignKey(fk => fk.CompanyUserId);


            #endregion

            #region Data Type Rules

            //modelBuilder.Entity<Product>().Property(x => x.Price).HasPrecision(18, 2);
           
            #endregion

            base.OnModelCreating(modelBuilder);

        }
    }
}
