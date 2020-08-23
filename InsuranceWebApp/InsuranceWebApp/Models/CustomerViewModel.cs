using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceWebApp.Models
{
    public class CustomerViewModel
    {
        [Key]
        public int CustomerId { get; set; }
        [Required]
        public string DocNumber { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public ICollection<PolicyViewModel> Policies { get; set; }
    }
}
