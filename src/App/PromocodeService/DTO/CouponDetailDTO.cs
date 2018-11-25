using PromocodeService.Enums;

namespace PromocodeService.DTO
{
    public class CouponDetailDTO
    {
        public DiscountType DiscountType { get; set; }

        public decimal DiscountAmount { get; set; }

        public string Couponcode { get; set; }

        public bool IsBulk { get; set; }
    }
}
