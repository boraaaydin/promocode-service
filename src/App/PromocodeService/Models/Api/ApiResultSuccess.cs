using System;

namespace PromocodeService.Models.Api
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
