using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRVacationSystemUI.Models
{
    public class APIResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}