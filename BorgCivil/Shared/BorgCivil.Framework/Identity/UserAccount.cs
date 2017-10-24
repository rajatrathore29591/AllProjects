namespace BorgCivil.Framework.Identity
{
    public class UserAccount
    {
        protected ApplicationUserManager manager = null;
        private static UserAccount _instance = null;
        public static UserAccount Create(ApplicationUserManager manager)
        {
            _instance = new UserAccount();
            _instance.manager = manager;
            return _instance;
        }

        public static UserAccount GetInstance()
        {
            return _instance;
        }

        public string Id
        {
            get
            {
                return manager.FindByEmailAsync(Email).Result.Id;
            }
        }

        public static string Email
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings.Get("IpmHubEmail");
            }
        }
    }
}
