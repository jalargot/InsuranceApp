using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceWebApp.Models
{
    public class PolicyViewModel
    {
        [Key]
        [Display(Name = "Policy Id")]
        public int PolicyId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Coverage { get; set; }
        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        [Display(Name = "Risk Type")]
        public RiskType RiskType { get; set; }
        [Display(Name = "Customer Id")]
        public int? CustomerId { get; set; }
    }
}
