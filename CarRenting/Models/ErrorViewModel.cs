using System;

namespace CarRenting.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        public string DateError { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}