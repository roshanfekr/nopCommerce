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

            this.Property(x => x.Code).HasMaxLength(50);
            this.Property(x => x.OldCode).HasMaxLength(50);
            this.Property(x => x.ProductName).HasMaxLength(200);
            this.Property(x => x.ProductEnName).HasMaxLength(200);
        }
    }
}