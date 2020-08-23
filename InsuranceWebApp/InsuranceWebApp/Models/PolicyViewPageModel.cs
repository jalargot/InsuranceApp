using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceWebApp.Models
{
    public class PolicyViewPageModel
    {
        public IEnumerable<PolicyViewModel> Policies { get; set; }

        [TempData]
        public string Message { get; set; }
    }
}
