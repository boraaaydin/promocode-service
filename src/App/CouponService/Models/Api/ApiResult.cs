using System;

namespace CouponService.Models.Api
{
    public class ApiResult
    {
        public ApiResult()
        {

        }

        public ApiResult(object entity, string message)
        {
            Entity = entity;
            Message = message;
        }

        public ApiResult(object entity, string message,bool isSuccess)
        {
            Entity = entity;
            Message = message;
            IsSuccess = isSuccess;
        }

        public Object Entity { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
    }
}
