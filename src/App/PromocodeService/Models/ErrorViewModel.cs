using System;

namespace PromocodeService.Models
{
    public class ErrorViewModel
    {
        public ErrorViewModel()
        {

        }
        public ErrorViewModel(string message, Exception ex = null)
        {
            ErrorMessage = message;
            Exception = ex;
        }

        public Exception Exception { get; set; }

        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string ErrorMessage { get; set; }
    }
}