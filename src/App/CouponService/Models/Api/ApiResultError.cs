using System;

namespace CouponService.Models.Api
{
    public class ApiResultError : ApiResult
    {
        public ApiResultError(string message)
        {
            Message = message;
            IsSuccess = false;
        }
    }
}
