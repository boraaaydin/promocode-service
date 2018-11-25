using PromocodeService.Domain.Models;
using PromocodeService.Enums;
using System;
using System.Collections.Generic;

namespace PromocodeService.DTO
{
    public class CouponManagementDTO
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DiscountType DiscountType { get; set; }
        public DateTime ExpireDate { get; set; }
        public bool IsInfinite { get; set; }
        public decimal DiscountAmount { get; set; }
        public string Code { get; set; }
        public bool IsForOneUse { get; set; }
        public bool IsBulk { get; set; }
        public List<CouponCode> Codes { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsCanceled { get; set; }
        public bool isForA { get; set; }
        public bool isForB { get; set; }
    }
}
