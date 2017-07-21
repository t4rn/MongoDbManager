using Autofac;
using Autofac.Integration.Mvc;
using RealEstate.DataAccess;
using RealEstate.Properties;
using RealEstate.Rentals;
using System.Web.Mvc;

namespace RealEstate.App_Start
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            //builder.RegisterFilterProvider();

            builder.RegisterSource(new ViewRegistrationSource());

            builder.RegisterModule(new AutofacModule());

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }

    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.Register(x => new RentalRepository(
                new DatabaseContext(
                connectionString: Settings.Default.csDB,
                databaseName: Settings.Default.dbName)))
                .As<IRentalRepository>().InstancePerRequest();

            builder.RegisterType<RentalService>().As<IRentalService>().InstancePerRequest();

            base.Load(builder);
        }
    }
}