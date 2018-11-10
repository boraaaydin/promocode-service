using CouponService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CouponService.EF.Configurations
{
    class CouponManagementConfiguration : IEntityTypeConfiguration<CouponManagement>
    {
        public void Configure(EntityTypeBuilder<CouponManagement> builder)
        {
            builder.Property(x => x.DiscountAmount).HasColumnType("decimal(11,2)");
        }
    }
}
