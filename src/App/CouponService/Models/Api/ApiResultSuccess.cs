using System;

namespace CouponService.Models.Api
{
    public class ApiResultSuccess : ApiResult
    {
        public ApiResultSuccess(object entity)
        {
            Entity = entity;
            Message = "success";
        }
    }
}
