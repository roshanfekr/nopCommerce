using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Misc.Hamkaran.Models
{
    public class HamkaranSettingModel : BaseNopModel
    {
        [NopResourceDisplayName("Plugins.Hamkaran.Config.Enable")]
        public bool Enable { get; set; }

        [NopResourceDisplayName("Plugins.Hamkaran.Config.UpdateRecallPeriod")]
        public int UpdateRecallPeriod { get; set; }

        [NopResourceDisplayName("Plugins.Hamkaran.Config.ConnectionString")]
        public string ConnectionString { get; set; }
    }
}