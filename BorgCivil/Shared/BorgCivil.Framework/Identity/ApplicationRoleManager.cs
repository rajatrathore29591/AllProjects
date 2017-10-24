//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;
//using Microsoft.AspNet.Identity.Owin;
//using Microsoft.Owin;

//namespace Ipm.Hub.Framework.Identity
//{
//    public class ApplicationRoleManager : RoleManager<ApplicationRole>
//    {
//        public ApplicationRoleManager(RoleStore<ApplicationRole> store)
//            : base(store)
//        {
//        }
//        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
//        {
//            var store = new RoleStore<ApplicationRole>(context.Get<AppIdentityDbContext>());
//            return new ApplicationRoleManager(store);
//        }
//    }
//}
