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
        public int PolicyId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Coverage { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public RiskType RiskType { get; set; }

        public int? CustomerId { get; set; }
    }
}
