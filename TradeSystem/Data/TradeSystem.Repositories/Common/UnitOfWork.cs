
using System.Transactions;
using System;
using TradeSystem.Framework.Identity;
using System.Data.Entity.Validation;
using TradeSystem.Framework.Entities;

namespace TradeSystem.Repositories
{
    public class UnitOfWork : AppIdentityDbContext, IUnitOfWork
    {
        public UnitOfWork()
        {
            InitializeRepositories();
        }


        public AppIdentityDbContext DbEntities
        {
            get
            {
                return ContextFactory.GetContext();
            }
        }

        /// <summary>
        /// This Repository for User Login Operations.
        /// </summary>
        public BankRepository BankRepository { get; private set; }

        /// <summary>
        ///  This Repository for Organisation & Organisation Entity Operations.
        /// </summary>
        public CustomerRepository CustomerRepository { get; private set; }

        /// <summary>
        ///  This Repository for Organisation & Organisation Entity Operations.
        /// </summary>
        public CustomerProductRepository CustomerProductRepository { get; private set; }

        /// <summary>
        ///  This Repository for Organisation & Organisation Entity Operations.
        /// </summary>
        public CompanyUserRepository CompanyUserRepository { get; private set; }

        /// <summary>
        /// This Repository for Organisation & OrganisationType Entity Operations.
        /// </summary>
        public ProductRepository ProductRepository { get; private set; }

        /// <summary>
        /// This Repository for User SkillCategory Operations.
        /// </summary>
        public RoleRepository RoleRepository { get; private set; }

        /// <summary>
        /// This Repository for User Skill Operations.
        /// </summary>
        public RoleSideMenuRepository<RoleSideMenu> RoleSideMenuRepository { get; private set; }

        /// <summary>
        /// This Repository for User SkillCategory Operations.
        /// </summary>
        public SideMenuRepository SideMenuRepository { get; private set; }

        /// <summary>
        /// This Repository for User Entity Operations.
        /// </summary>
        public TicketRepository TicketRepository { get; private set; }

        /// <summary>
        /// This Repository for ProjectRole Entity Operations.
        /// </summary>
        public WithdrawRepository WithdrawRepository { get; private set; }

        ///// <summary>
        /////  This Repository for Document Entity Operations.
        ///// </summary>
        public EmailRepository EmailRepository { get; private set; }

        ///// <summary>
        ///// This Repository for Penalty Entity Operations.
        ///// </summary>
        public PenaltyRepository PenaltyRepository { get; private set; }

        ///// <summary>
        ///// This Repository for UserQualification Entity .
        ///// </summary>
        public DocumentRepository DocumentRepository { get; private set; }

        public CountryRepository CountryRepository { get; private set; }

        public StateRepository StateRepository { get; private set; }

        public ActivityLogRepository ActivityLogRepository { get; private set; }

        public PromotionRepository PromotionRepository { get; private set; }
        public TicketStatusRepository TicketStatusRepository { get; private set; }

        //This Repository for Wallet Entity Operations.
        public WalletRepository WalletRepository { get; private set; }

        /// <summary>
        /// This method user for save changes in database.
        /// </summary>
        public void Commit()
        {
            try
            {
                SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                Exception raise = ex;
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }

        /// <summary>
        /// This Method use for save change with transaction with database.
        /// </summary>
        public void CommitTransaction()
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                Commit();
                transactionScope.Complete();
            }
        }

        private void InitializeRepositories()
        {
            BankRepository = new BankRepository();
            CustomerRepository = new CustomerRepository();
            CustomerProductRepository = new CustomerProductRepository();
            CompanyUserRepository = new CompanyUserRepository();
            ProductRepository = new ProductRepository();
            RoleRepository = new RoleRepository();
            RoleSideMenuRepository = new RoleSideMenuRepository<RoleSideMenu>();
            SideMenuRepository = new SideMenuRepository();
            TicketRepository = new TicketRepository();
            WithdrawRepository = new WithdrawRepository();
            EmailRepository = new EmailRepository();
            EmailRepository = new EmailRepository();
            PenaltyRepository = new PenaltyRepository();
            DocumentRepository = new DocumentRepository();
            CountryRepository = new CountryRepository();
            StateRepository = new StateRepository();
            ActivityLogRepository = new ActivityLogRepository();
            PromotionRepository = new PromotionRepository();
            TicketStatusRepository = new TicketStatusRepository();
            WalletRepository = new WalletRepository();

        }


    }
}
