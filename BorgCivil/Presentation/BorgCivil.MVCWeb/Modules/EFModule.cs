using Autofac;
using System.Data.Entity;
using BorgCivil.Framework.Identity;
using BorgCivil.Repositories;

namespace BorgCivil.MVCWeb.Modules
{
    public class EFModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(AppIdentityDbContext)).As(typeof(DbContext)).InstancePerLifetimeScope();
            builder.RegisterType(typeof(UnitOfWork)).As(typeof(IUnitOfWork)).InstancePerRequest();
            
        }

    }
}
