using System.Web.Mvc;
using System.Web.Routing;
using Nop.Web.Framework.Mvc.Routes;

namespace Nop.Plugin.Misc.Hamkaran.ByWeight
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            //routes.MapRoute("Plugin.Shipping.ByWeight.SaveGeneralSettings",
            //     "Plugins/ShippingByWeight/SaveGeneralSettings",
            //     new { controller = "ShippingByWeight", action = "SaveGeneralSettings", },
            //     new[] { "Nop.Plugin.Misc.Hamkaran.ByWeight.Controllers" }
            //);

            //routes.MapRoute("Plugin.Shipping.ByWeight.AddPopup",
            //     "Plugins/ShippingByWeight/AddPopup",
            //     new { controller = "ShippingByWeight", action = "AddPopup" },
            //     new[] { "Nop.Plugin.Misc.Hamkaran.ByWeight.Controllers" }
            //);
            //routes.MapRoute("Plugin.Shipping.ByWeight.EditPopup",
            //     "Plugins/ShippingByWeight/EditPopup",
            //     new { controller = "ShippingByWeight", action = "EditPopup" },
            //     new[] { "Nop.Plugin.Misc.Hamkaran.ByWeight.Controllers" }
            //);
        }
        public int Priority
        {
            get
            {
                return 0;
            }
        }
    }
}
