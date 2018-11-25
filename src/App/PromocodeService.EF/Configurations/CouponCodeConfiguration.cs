using PromocodeService.Domain;
using PromocodeService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PromocodeService.EF.Configurations
{
    class CouponCodeConfiguration : IEntityTypeConfiguration<CouponCode>
    {
        public void Configure(EntityTypeBuilder<CouponCode> builder)
        {
            builder.Property(x => x.Code).IsRequired();

            builder.HasIndex(x => x.Code).IsUnique(true);
        }
    }
}
