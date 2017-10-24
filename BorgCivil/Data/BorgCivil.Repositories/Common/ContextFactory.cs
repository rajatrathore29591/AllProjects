using BorgCivil.Framework.Identity;

namespace BorgCivil.Repositories
{
    public class ContextFactory
    {
        public static AppIdentityDbContext GetContext()
        {
            return new AppIdentityDbContext();
        }
    }
}
