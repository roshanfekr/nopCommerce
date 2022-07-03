using Nop.Data.Mapping;
using Nop.Plugin.Misc.Hamkaran.Domain;

namespace Nop.Plugin.Misc.Hamkaran.Data
{
    public partial class HamkaranProductRecordMap : NopEntityTypeConfiguration<HamkaranProductRecord>
    {
        public HamkaranProductRecordMap()
        {
            this.ToTable("HamkaranProduct");
            this.HasKey(x => x.Id);

            this.Property(x => x.Zip).HasMaxLength(400);
        }
    }
}