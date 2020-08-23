using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceWebApp.Models
{
    public class CustomerCreatePageModel
    {
        [BindProperty]
        public CustomerViewModel Customer { get; set; }

    }
}
