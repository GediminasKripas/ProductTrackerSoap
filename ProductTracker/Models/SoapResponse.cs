using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductTracker.Models
{
    public class SoapResponse<T>
    {
        public T? obj {get;set;}
        public int code { get; set; }
    }
}
