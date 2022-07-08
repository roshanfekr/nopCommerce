using System;
using System.Web.Routing;
using Nop.Core;
using Nop.Core.Domain.Shipping;
using Nop.Core.Domain.Tasks;
using Nop.Core.Plugins;
using Nop.Plugin.Misc.Hamkaran.Data;
using Nop.Plugin.Misc.Hamkaran.Services;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Shipping;
using Nop.Services.Shipping.Tracking;
using Nop.Services.Tasks;

namespace Nop.Plugin.Misc.Hamkaran
{
    public class HamkaranTransferPlugin : BasePlugin, IMiscPlugin
    {
        #region Fields
        public static string ScheduleTaskType => "Nop.Plugin.Misc.Hamkaran.Services.HamkaranSchedule";

        private readonly IShippingService _shippingService;
        private readonly IStoreContext _storeContext;
        private readonly IHamkaranTransferService _hamkaranTransferService;
        private readonly IPriceCalculationService _priceCalculationService;
        private readonly HamkaranSettings _hamkaranSettings;
        private readonly HamkaranTransferObjectContext _objectContext;
        private readonly ISettingService _settingService;
        private readonly IScheduleTaskService _scheduleTaskService;
        #endregion

        #region Ctor
        public HamkaranTransferPlugin(IShippingService shippingService,
            IStoreContext storeContext,
            IHamkaranTransferService hamkaranTransferService,
            IScheduleTaskService scheduleTaskService,
            IPriceCalculationService priceCalculationService,
            HamkaranSettings hamkaranSettings,
            HamkaranTransferObjectContext objectContext,
            ISettingService settingService)
        {
            this._shippingService = shippingService;
            this._storeContext = storeContext;
            this._hamkaranTransferService = hamkaranTransferService;
            this._priceCalculationService = priceCalculationService;
            this._hamkaranSettings = hamkaranSettings;
            this._objectContext = objectContext;
            this._settingService = settingService;
            this._scheduleTaskService = scheduleTaskService;


            if (_scheduleTaskService.GetTaskByType(ScheduleTaskType) == null)
            {
                _scheduleTaskService.InsertTask(new ScheduleTask
                {
                    Enabled = true,
                    Seconds = 60 * 60,
                    Name = "Hamkaran data transfer",
                    Type = ScheduleTaskType,
                });
            }

        }
        #endregion



        #region Methods

        /// <summary>
        /// Install plugin
        /// </summary>
        public override void Install()
        {
            //settings
            var settings = new HamkaranSettings
            {
                Enable = false,
                UpdateRecallPeriod = 3600
            };
            _settingService.SaveSetting(settings);



            //database objects
            _objectContext.Install();

            //locales

            this.AddOrUpdatePluginLocaleResource("Admin.Common.TestConnection", "تست اتصال");
            this.AddOrUpdatePluginLocaleResource("Plugins.Hamkaran.Config.Enable", "وضعیت پلاگین");
            this.AddOrUpdatePluginLocaleResource("Plugins.Hamkaran.Config.UpdateRecallPeriod", "دروه زمانی اتصال به سرور(ثانیه)");
            this.AddOrUpdatePluginLocaleResource("Plugins.Hamkaran.Config.ConnectionString", "رشته اتصال به سرور");
            this.AddOrUpdatePluginLocaleResource("Plugins.Hamkaran.Config.TestConnection", "رشته اتصال به سرور");


            //this.AddOrUpdatePluginLocaleResource("Plugins.Shipping.ByWeight.Fields.Store", "Store");
            //this.AddOrUpdatePluginLocaleResource("Plugins.Shipping.ByWeight.Fields.Store.Hint", "If an asterisk is selected, then this shipping rate will apply to all stores.");
            //this.AddOrUpdatePluginLocaleResource("Plugins.Shipping.ByWeight.Fields.Warehouse", "Warehouse");
            //this.AddOrUpdatePluginLocaleResource("Plugins.Shipping.ByWeight.Fields.Warehouse.Hint", "If an asterisk is selected, then this shipping rate will apply to all warehouses.");
            //this.AddOrUpdatePluginLocaleResource("Plugins.Shipping.ByWeight.Fields.Country", "Country");
            //this.AddOrUpdatePluginLocaleResource("Plugins.Shipping.ByWeight.Fields.Country.Hint", "If an asterisk is selected, then this shipping rate will apply to all customers, regardless of the country.");
            //this.AddOrUpdatePluginLocaleResource("Plugins.Shipping.ByWeight.Fields.StateProvince", "State / province");
            //this.AddOrUpdatePluginLocaleResource("Plugins.Shipping.ByWeight.Fields.StateProvince.Hint", "If an asterisk is selected, then this shipping rate will apply to all customers from the given country, regardless of the state.");
            //this.AddOrUpdatePluginLocaleResource("Plugins.Shipping.ByWeight.Fields.Zip", "Zip");
            //this.AddOrUpdatePluginLocaleResource("Plugins.Shipping.ByWeight.Fields.Zip.Hint", "Zip / postal code. If zip is empty, then this shipping rate will apply to all customers from the given country or state, regardless of the zip code.");
            //this.AddOrUpdatePluginLocaleResource("Plugins.Shipping.ByWeight.Fields.ShippingMethod", "Shipping method");
            //this.AddOrUpdatePluginLocaleResource("Plugins.Shipping.ByWeight.Fields.ShippingMethod.Hint", "The shipping method.");
            //this.AddOrUpdatePluginLocaleResource("Plugins.Shipping.ByWeight.Fields.From", "Order weight from");
            //this.AddOrUpdatePluginLocaleResource("Plugins.Shipping.ByWeight.Fields.From.Hint", "Order weight from.");
            //this.AddOrUpdatePluginLocaleResource("Plugins.Shipping.ByWeight.Fields.To", "Order weight to");
            //this.AddOrUpdatePluginLocaleResource("Plugins.Shipping.ByWeight.Fields.To.Hint", "Order weight to.");
            //this.AddOrUpdatePluginLocaleResource("Plugins.Shipping.ByWeight.Fields.AdditionalFixedCost", "Additional fixed cost");
            //this.AddOrUpdatePluginLocaleResource("Plugins.Shipping.ByWeight.Fields.AdditionalFixedCost.Hint", "Specify an additional fixed cost per shopping cart for this option. Set to 0 if you don't want an additional fixed cost to be applied.");
            //this.AddOrUpdatePluginLocaleResource("Plugins.Shipping.ByWeight.Fields.LowerWeightLimit", "Lower weight limit");
            //this.AddOrUpdatePluginLocaleResource("Plugins.Shipping.ByWeight.Fields.LowerWeightLimit.Hint", "Lower weight limit. This field can be used for \"per extra weight unit\" scenarios.");
            //this.AddOrUpdatePluginLocaleResource("Plugins.Shipping.ByWeight.Fields.PercentageRateOfSubtotal", "Charge percentage (of subtotal)");
            //this.AddOrUpdatePluginLocaleResource("Plugins.Shipping.ByWeight.Fields.PercentageRateOfSubtotal.Hint", "Charge percentage (of subtotal).");
            //this.AddOrUpdatePluginLocaleResource("Plugins.Shipping.ByWeight.Fields.RatePerWeightUnit", "Rate per weight unit");
            //this.AddOrUpdatePluginLocaleResource("Plugins.Shipping.ByWeight.Fields.RatePerWeightUnit.Hint", "Rate per weight unit.");
            //this.AddOrUpdatePluginLocaleResource("Plugins.Shipping.ByWeight.Fields.LimitMethodsToCreated", "Limit shipping methods to configured ones");
            //this.AddOrUpdatePluginLocaleResource("Plugins.Shipping.ByWeight.Fields.LimitMethodsToCreated.Hint", "If you check this option, then your customers will be limited to shipping options configured here. Otherwise, they'll be able to choose any existing shipping options even they've not configured here (zero shipping fee in this case).");
            //this.AddOrUpdatePluginLocaleResource("Plugins.Shipping.ByWeight.Fields.DataHtml", "Data");
            //this.AddOrUpdatePluginLocaleResource("Plugins.Shipping.ByWeight.AddRecord", "Add record");
            //this.AddOrUpdatePluginLocaleResource("Plugins.Shipping.ByWeight.Formula", "Formula to calculate rates");
            //this.AddOrUpdatePluginLocaleResource("Plugins.Shipping.ByWeight.Formula.Value", "[additional fixed cost] + ([order total weight] - [lower weight limit]) * [rate per weight unit] + [order subtotal] * [charge percentage]");

            base.Install();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<HamkaranSettings>();

            //database objects
            _objectContext.Uninstall();

            //locales
            //this.DeletePluginLocaleResource("Plugins.Shipping.ByWeight.Fields.Store");
            this.DeletePluginLocaleResource("Admin.Common.TestConnection");
            this.DeletePluginLocaleResource("Plugins.Hamkaran.Config.Enable");
            this.DeletePluginLocaleResource("Plugins.Hamkaran.Config.UpdateRecallPeriod");
            this.DeletePluginLocaleResource("Plugins.Hamkaran.Config.ConnectionString");
            base.Uninstall();
        }

        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "HamkaranTransfer";
            routeValues = new RouteValueDictionary { { "Namespaces", "Nop.Plugin.Misc.Hamkaran.Controllers" }, { "area", null } };
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a shipping rate computation method type
        /// </summary>
        public ShippingRateComputationMethodType ShippingRateComputationMethodType
        {
            get
            {
                return ShippingRateComputationMethodType.Offline;
            }
        }


        /// <summary>
        /// Gets a shipment tracker
        /// </summary>
        public IShipmentTracker ShipmentTracker
        {
            get
            {
                //uncomment a line below to return a general shipment tracker (finds an appropriate tracker by tracking number)
                //return new GeneralShipmentTracker(EngineContext.Current.Resolve<ITypeFinder>());
                return null; 
            }
        }

        #endregion
    }
}
