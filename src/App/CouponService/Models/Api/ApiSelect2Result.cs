using System.Collections.Generic;

namespace CouponService.Models.Api
{
    public class ApiSelect2Result
    {
        public ApiSelect2Result()
        {
            Results = new List<ApiSelect2DTO>();
        }
        public List<ApiSelect2DTO> Results { get; set; }
        public string Message { get; set; }
    }
}
