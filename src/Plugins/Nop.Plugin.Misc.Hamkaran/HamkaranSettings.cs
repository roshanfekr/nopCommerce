
using Nop.Core.Configuration;

namespace Nop.Plugin.Misc.Hamkaran
{
    public class HamkaranSettings : ISettings
    {
        public bool Enable { get; set; }

        public int UpdateRecallPeriod { get; set; }

        public string ConnectionString { get; set; }

    }
}