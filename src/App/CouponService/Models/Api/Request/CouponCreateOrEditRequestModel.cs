using System;
using CouponService.Enums;

namespace CouponService.Models.Api.Request
{
    public class CouponCreateOrEditRequestModel
    {
        public int Id { get; internal set; }
        public string Description { get; internal set; }
        public decimal DiscountAmount { get; internal set; }
        public DiscountType DiscountType { get; internal set; }
        public bool IsInfinite { get; internal set; }
        public bool IsBulk { get; internal set; }
        public bool IsForOneUse { get; internal set; }
        public bool isForB { get; internal set; }
        public bool isForA { get; internal set; }
        public string Code { get; internal set; }
        public bool IsActive { get; internal set; }
        public DateTime ExpireDate { get; internal set; }
        public int Quantity { get; set; }
        public int CouponLength { get; set; }
    }
}
