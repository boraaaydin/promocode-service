namespace CouponService.Domain.Models
{
    public class CouponCode
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public int CouponManagementId { get; set; }

        public CouponManagement CouponManagement { get; set; }

        public bool IsActive { get; set; }

        public bool IsUsed { get; set; }
    }
}
