using CouponService.Enums;

namespace CouponService.DTO
{
    public class CouponDetailDTO
    {
        public DiscountType DiscountType { get; set; }

        public decimal DiscountAmount { get; set; }

        public string Couponcode { get; set; }

        public bool IsBulk { get; set; }
    }
}
