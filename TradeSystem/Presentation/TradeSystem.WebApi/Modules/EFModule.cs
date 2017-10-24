using Autofac;
using System.Data.Entity;
using TradeSystem.Framework.Identity;
using TradeSystem.Repositories;

namespace TradeSystem.MVCWeb.Modules
{
    public class EFModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(AppIdentityDbContext)).As(typeof(DbContext)).InstancePerLifetimeScope();
            //builder.RegisterType(typeof(TangoBMSIdentityDbContext)).As(typeof(DbContext)).InstancePerLifetimeScope();
            builder.RegisterType(typeof(UnitOfWork)).As(typeof(IUnitOfWork)).InstancePerRequest();
            
        }

    }
}
