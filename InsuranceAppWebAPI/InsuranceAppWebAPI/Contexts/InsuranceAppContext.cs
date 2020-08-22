using InsuranceAppWebAPI.Models;
using Microsoft.EntityFrameworkCore;

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
        }

        public DbSet<Policy> Policies { get; set; }
        public DbSet<Customer> Customers { get; set; }

    }
}
