using System;
using System.ComponentModel.DataAnnotations;

namespace InsuranceAppWebAPI.Models
{
    public class Policy
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
        public Customer Customer { get; set; }
    }
}
