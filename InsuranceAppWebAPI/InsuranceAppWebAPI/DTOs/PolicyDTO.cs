using InsuranceAppWebAPI.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace InsuranceAppWebAPI.DTOs
{
    public class PolicyDTO
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
        // removed because is causing a loop
        // public CustomerDTO Customer { get; set; }
    }
}
