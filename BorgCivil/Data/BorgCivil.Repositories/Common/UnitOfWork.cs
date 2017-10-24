
using System.Transactions;
using System;
using BorgCivil.Framework.Identity;
using System.Data.Entity.Validation;

namespace BorgCivil.Repositories
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
        /// This Repository for Attachments Operations.
        /// </summary>
        public AnonymousFieldRepository AnonymousFieldRepository { get; private set; }

        /// <summary>
        /// This Repository for Attachments Operations.
        /// </summary>
        public AttachmentsRepository AttachmentsRepository { get; private set; }

        /// <summary>
        /// This Repository for Booking Operations.
        /// </summary>
        public BookingRepository BookingRepository { get; private set; }

        /// <summary>
        /// This Repository for BookingFleets Operations.
        /// </summary>
        public BookingFleetsRepository BookingFleetsRepository { get; private set; }

        /// <summary>
        /// This Repository for BookingSiteGate Operations.
        /// </summary>
        public BookingSiteGatesRepository BookingSiteGatesRepository { get; private set; }

        /// <summary>
        /// This Repository for BookingSiteSupervisor Operations.
        /// </summary>
        public BookingSiteSupervisorRepository BookingSiteSupervisorRepository { get; private set; }

        /// <summary>
        /// This Repository for Customer Operations.
        /// </summary>
        public CustomerRepository CustomerRepository { get; private set; }

        /// <summary>
        /// This Repository for Country Operations.
        /// </summary>
        public CountryRepository CountryRepository { get; private set; }

        /// <summary>
        /// This Repository for Driver Operations.
        /// </summary>
        public DriversRepository DriversRepository { get; private set; }

        /// <summary>
        /// This Repository for DriverWhiteCard Operations.
        /// </summary>
        public DriverWhiteCardRepository DriverWhiteCardRepository { get; private set; }

        /// <summary>
        /// This Repository for DriverInductionCard Operations.
        /// </summary>
        public DriverInductionCardRepository DriverInductionCardRepository { get; private set; }

        /// <summary>
        /// This Repository for DriverVocCard Operations.
        /// </summary>
        public DriverVocCardRepository DriverVocCardRepository { get; private set; }

        /// <summary>
        /// This Repository for Docket Operations.
        /// </summary>
        public DocketRepository DocketRepository { get; private set; }

        /// <summary>
        /// This Repository for Document Operations.
        /// </summary>
        public DocumentRepository DocumentRepository { get; private set; }

        /// <summary>
        /// This Repository for DocketCheckList Operations.
        /// </summary>
        public DocketCheckListRepository DocketCheckListRepository { get; private set; }

        /// <summary>
        /// This Repository for Email Operations.
        /// </summary>
        public EmailRepository EmailRepository { get; private set; }

        /// <summary>
        /// This Repository for Employee Operations.
        /// </summary>
        public EmployeeRepository EmployeeRepository { get; private set; }

        /// <summary>
        /// This Repository for Employee Operations.
        /// </summary>
        public EmploymentCategoryRepository EmploymentCategoryRepository { get; private set; }

        /// <summary>
        /// This Repository for Fleet Operations.
        /// </summary>
        public FleetRepository FleetRepository { get; private set; }

        /// <summary>
        /// This Repository for FleetHistory Operations.
        /// </summary>
        public FleetHistoryRepository FleetHistoryRepository { get; private set; }

        /// <summary>
        /// This Repository for FleetType Operations.
        /// </summary>
        public FleetTypesRepository FleetTypesRepository { get; private set; }

        /// <summary>
        /// This Repository for Fleet Registration Operations.
        /// </summary>
        public FleetsRegistrationRepository FleetsRegistrationRepository { get; private set; }

        /// <summary>
        /// This Repository for Gates Operations.
        /// </summary>
        public GatesRepository GatesRepository { get; private set; }

        /// <summary>
        /// This Repository for GateContactPerson Operations.
        /// </summary>
        public GateContactPersonRepository GateContactPersonRepository { get; private set; }

        /// <summary>
        /// This Repository for LicenseClass Operations.
        /// </summary>
        public LicenseClassRepository LicenseClassRepository { get; private set; }

        /// <summary>
        /// This Repository for LoadDocket Operations.
        /// </summary>
        public LoadDocketRepository LoadDocketRepository { get; private set; }

        /// <summary>
        /// This Repository for Sites Operations.
        /// </summary>
        public SitesRepository SitesRepository { get; private set; }

        /// <summary>
        /// This Repository for Supervisor Operations.
        /// </summary>
        public SupervisorRepository SupervisorRepository { get; private set; }

        /// <summary>
        /// This Repository for StatusLookup Operations.
        /// </summary>
        public StatusLookupRepository StatusLookupRepository { get; private set; }

        /// <summary>
        /// This Repository for State Operations.
        /// </summary>
        public StateRepository StateRepository { get; private set; }

        /// <summary>
        /// This Repository for Subcontractor Operations.
        /// </summary>
        public SubcontractorRepository SubcontractorRepository { get; private set; }

        /// <summary>
        /// This Repository for WorkTypes Operations.
        /// </summary>
        public WorkTypesRepository WorkTypesRepository { get; private set; }

        /// This Repository for WorkTypes Operations.
        /// </summary>
        public DemoRepository DemoRepository { get; private set; }



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
            AnonymousFieldRepository = new AnonymousFieldRepository();
            AttachmentsRepository = new AttachmentsRepository();
            BookingRepository = new BookingRepository();
            BookingFleetsRepository = new BookingFleetsRepository();
            BookingSiteGatesRepository = new BookingSiteGatesRepository();
            BookingSiteSupervisorRepository = new BookingSiteSupervisorRepository();
            CustomerRepository = new CustomerRepository();
            CountryRepository = new CountryRepository();
            DriversRepository = new DriversRepository();
            DriverWhiteCardRepository = new DriverWhiteCardRepository();
            DriverInductionCardRepository = new DriverInductionCardRepository();
            DriverVocCardRepository = new DriverVocCardRepository();
            DocketRepository = new DocketRepository();
            DocumentRepository = new DocumentRepository();
            DocketCheckListRepository = new DocketCheckListRepository();
            EmployeeRepository = new EmployeeRepository();
            EmploymentCategoryRepository = new EmploymentCategoryRepository();
            EmailRepository = new EmailRepository();
            FleetRepository = new FleetRepository();
            FleetHistoryRepository = new FleetHistoryRepository();
            FleetTypesRepository = new FleetTypesRepository();
            FleetsRegistrationRepository = new FleetsRegistrationRepository();
            GatesRepository = new GatesRepository();
            GateContactPersonRepository = new GateContactPersonRepository();
            LicenseClassRepository = new LicenseClassRepository();
            LoadDocketRepository = new LoadDocketRepository();
            SitesRepository = new SitesRepository();
            SupervisorRepository = new SupervisorRepository();
            StatusLookupRepository = new StatusLookupRepository();
            StateRepository = new StateRepository();
            WorkTypesRepository = new WorkTypesRepository();
            SubcontractorRepository = new SubcontractorRepository();
            DemoRepository = new DemoRepository();
            //TicketRepository = new TicketRepository();
            //WithdrawRepository = new WithdrawRepository();
          
            //EmailRepository = new EmailRepository();
            //PenaltyRepository = new PenaltyRepository();
            //DocumentRepository = new DocumentRepository();
            //CountryRepository = new CountryRepository();
            //StateRepository = new StateRepository();
            //ActivityLogRepository = new ActivityLogRepository();
            //PromotionRepository = new PromotionRepository();
            //TicketStatusRepository = new TicketStatusRepository();
            //WalletRepository = new WalletRepository();

        }


    }
}
