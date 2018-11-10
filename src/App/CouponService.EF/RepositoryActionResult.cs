
using System;

namespace CouponService.EF
{
    public class RepositoryActionResult<T> where T : class
    {
        public RepositoryActionStatus Status { get; set; }
        public T Entity { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; private set; }

        public RepositoryActionResult()
        {

        }
        public RepositoryActionResult(RepositoryActionStatus status, T entity, string message = "")
        {
            Status = status;
            Entity = entity;
            Message = message;
        }

        public RepositoryActionResult(RepositoryActionStatus status, T entity, Exception exception) : this(status, entity, "")
        {
            Exception = exception;
        }

    }
}
