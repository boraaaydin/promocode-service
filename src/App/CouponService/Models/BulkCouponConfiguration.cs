using System;
using System.Collections.Generic;
using System.Text;

namespace CouponService.Models
{
    public class BulkCouponConfiguration
    {
        public int Quantity { get; set; }
        public int Length { get; set; } = 10;
    }
}
