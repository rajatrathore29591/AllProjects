using TradeSystem.Framework.Identity;

namespace TradeSystem.Repositories
{
    public class ContextFactory
    {
        public static AppIdentityDbContext GetContext()
        {
            return new AppIdentityDbContext();
        }
    }
}
