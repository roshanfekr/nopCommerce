using Autofac;
using Autofac.Core;
using Nop.Core.Configuration;
using Nop.Core.Data;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Data;
using Nop.Plugin.Misc.Hamkaran.Data;
using Nop.Plugin.Misc.Hamkaran.Domain;
using Nop.Plugin.Misc.Hamkaran.Services;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Misc.Hamkaran.ByWeight
{
    /// <summary>
    /// Dependency registrar
    /// </summary>
    public class DependencyRegistrar : IDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="config">Config</param>
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            builder.RegisterType<HamkaranTransferService>().As<IHamkaranTransferService>().InstancePerLifetimeScope();

            //data context
            this.RegisterPluginDataContext<HamkaranTransferObjectContext>(builder, "nop_object_context_shipping_weight_zip");

            ////override required repository with our custom context
            //builder.RegisterType<EfRepository<ShippingByWeightRecord>>()
            //    .As<IRepository<ShippingByWeightRecord>>()
            //    .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_shipping_weight_zip"))
            //    .InstancePerLifetimeScope();
        }

        /// <summary>
        /// Order of this dependency registrar implementation
        /// </summary>
        public int Order
        {
            get { return 1; }
        }
    }
}
