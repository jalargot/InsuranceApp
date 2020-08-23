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
        [Display(Name = "Customer Id")]
        public int CustomerId { get; set; }
        [Required]
        [Display(Name = "Doc Number")]
        public string DocNumber { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public ICollection<PolicyViewModel> Policies { get; set; }
    }
}
