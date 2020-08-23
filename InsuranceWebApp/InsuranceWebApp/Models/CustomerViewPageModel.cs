using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceWebApp.Models
{
    public class CustomerViewPageModel
    {
        public IEnumerable<CustomerViewModel> Customers { get; set; }

        [TempData]
        public string Message { get; set; }
    }
}
