using Nop.Core;
using System;

namespace Nop.Plugin.Misc.Hamkaran.Domain
{
    /// <summary>
    /// Represents a shipping by weight record
    /// </summary>
    public partial class HamkaranProductRecord : BaseEntity
    {
        /// <summary>
        /// [کد کالا]
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// کد قدیمی
        /// </summary>
        public string OldCode { get; set; }

        /// <summary>
        /// [نام کالا]
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// [نام انگلیسی]
        /// </summary>
        public string ProductEnName { get; set; }

        /// <summary>
        /// زمان بروز رسانی
        /// </summary>
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// فعال
        /// </summary>
        public bool Active { get; set; }


    }
}