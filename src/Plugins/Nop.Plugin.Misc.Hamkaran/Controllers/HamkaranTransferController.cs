using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Domain.Directory;
using Nop.Plugin.Misc.Hamkaran.Domain;
using Nop.Plugin.Misc.Hamkaran.Models;
using Nop.Services.Configuration;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Services.Shipping;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Security;

namespace Nop.Plugin.Misc.Hamkaran.Controllers
{
    [AdminAuthorize]
    public class HamkaranTransferController : BasePluginController
    {
        private readonly IShippingService _shippingService;
        private readonly IStoreService _storeService;
        private readonly ICountryService _countryService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly HamkaranSettings _hamkaranSettings;
        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;

        private readonly ICurrencyService _currencyService;
        private readonly CurrencySettings _currencySettings;
        private readonly IMeasureService _measureService;
        private readonly MeasureSettings _measureSettings;

        public HamkaranTransferController(IShippingService shippingService,
            IStoreService storeService,
            ICountryService countryService,
            IStateProvinceService stateProvinceService,
            HamkaranSettings hamkaranSettings,
            ISettingService settingService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            ICurrencyService currencyService,
            CurrencySettings currencySettings,
            IMeasureService measureService,
            MeasureSettings measureSettings)
        {
            this._shippingService = shippingService;
            this._storeService = storeService;
            this._countryService = countryService;
            this._stateProvinceService = stateProvinceService;
            this._hamkaranSettings = hamkaranSettings;
            this._settingService = settingService;
            this._localizationService = localizationService;
            this._permissionService = permissionService;
            this._currencyService = currencyService;
            this._currencySettings = currencySettings;
            this._measureService = measureService;
            this._measureSettings = measureSettings;
        }
        
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            //little hack here
            //always set culture to 'en-US' (Telerik has a bug related to editing decimal values in other cultures). Like currently it's done for admin area in Global.asax.cs
            CommonHelper.SetTelerikCulture();

            base.Initialize(requestContext);
        }

        [ChildActionOnly]
        [AdminAuthorize]
        public ActionResult Configure()
        {
            var model = new HamkaranSettingModel();
            //other settings
            model.Enable = _hamkaranSettings.Enable;
            model.UpdateRecallPeriod = _hamkaranSettings.UpdateRecallPeriod;
            model.ConnectionString = _hamkaranSettings.ConnectionString;


            return View("~/Plugins/Misc.Hamkaran/Views/Configure.cshtml", model);
        }

        [HttpPost]
        [AdminAntiForgery]
        [FormValueRequired("save")]
        [AdminAuthorize]

        public ActionResult SaveGeneralSettings(HamkaranSettingModel model)
        {
            //save settings
            _hamkaranSettings.Enable = model.Enable;
            _hamkaranSettings.UpdateRecallPeriod = model.UpdateRecallPeriod;
            _hamkaranSettings.ConnectionString = model.ConnectionString;

            _settingService.SaveSetting(_hamkaranSettings);

            return Json(new { Result = true });
        }


        [AdminAntiForgery]
        [ChildActionOnly]
        [FormValueRequired("testconnection")]
        [AdminAuthorize]
        public ActionResult Configure(HamkaranSettingModel model)
        {

            model.ConnectionOk = HamkaranSettingModel.ConnectionTest.TestOK;

            return View("~/Plugins/Misc.Hamkaran/Views/Configure.cshtml", model);
        }

    }
}
