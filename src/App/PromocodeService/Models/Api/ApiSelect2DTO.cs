
using System;
using System.Collections.Generic;
using System.Text;

namespace PromocodeService.Models.Api
{
    public class ApiSelect2DTO
    {
        public string Id { get; set; }
        public string Text { get; set; }

        public decimal DiscountAmount { get; set; }
        public int DiscountType { get; set; }
    }
}
