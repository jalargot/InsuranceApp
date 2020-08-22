using InsuranceAppWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace InsuranceAppWebAPI.Contexts
{
    public class InsuranceAppContext : DbContext
    {
        public InsuranceAppContext(DbContextOptions<InsuranceAppContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    CustomerId = 1,
                    DocNumber = "1234567890",
                    FirstName = "Luke",
                    LastName = "Skywalker",
                    Email = "jedi@email.com",
                    Phone = "987654321",
                    Address = "123 death star avenue"
                },
                 new Customer
                 {
                     CustomerId = 2,
                     DocNumber = "2143658709",
                     FirstName = "Leia",
                     LastName = "Skywalker",
                     Email = "pricess@email.com",
                     Phone = "896745231",
                     Address = "123 death star avenue"
                 }
            );

            modelBuilder.Entity<Policy>(entity =>
            {
                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Policies)
                    .HasForeignKey("CustomerId");
            });

            modelBuilder.Entity<Policy>().HasData(
                new Policy
                {
                    PolicyId = 1,
                    Name = "Name1",
                    Description = "Description1",
                    Coverage = 70,
                    StartDate = DateTime.Now,
                    Duration = 12,
                    Price = 1200000,
                    RiskType = RiskType.Medium_High,
                    CustomerId = 1
                },
                 new Policy
                 {
                     PolicyId = 2,
                     Name = "Name2",
                     Description = "Description2",
                     Coverage = 40,
                     StartDate = DateTime.Now,
                     Duration = 24,
                     Price = 4000000,
                     RiskType = RiskType.High,
                     CustomerId = 2
                 }
            );

        }

        public DbSet<Policy> Policies { get; set; }
        public DbSet<Customer> Customers { get; set; }

    }
}
