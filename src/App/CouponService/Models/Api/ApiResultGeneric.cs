using System;

namespace CouponService.Models.Api
{
    public class ApiResultGeneric<T> where T : class 
    {
        public T Entity { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }

        public ApiResultGeneric()
        {

        }

        public ApiResultGeneric(bool isSuccess,T entity,string message)
        {
            IsSuccess = isSuccess;
            Message = message;
            Entity = entity;
        }
    }
}
