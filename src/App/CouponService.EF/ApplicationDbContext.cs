using CouponService.Domain.Models;
using CouponService.EF.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CouponService.EF
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<CouponCode> CouponCodes { get; set; }
        public DbSet<CouponManagement> CouponManagements { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new CouponManagementConfiguration());
            builder.ApplyConfiguration(new CouponCodeConfiguration());

            base.OnModelCreating(builder);
        }
    }
}
