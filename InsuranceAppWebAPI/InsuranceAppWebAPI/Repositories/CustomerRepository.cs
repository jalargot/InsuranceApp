using InsuranceAppWebAPI.Contexts;
using InsuranceAppWebAPI.Exceptions;
using InsuranceAppWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InsuranceAppWebAPI.Repositories
{
    public class CustomerRepository
    {
        private readonly InsuranceAppContext _context;

        public CustomerRepository(InsuranceAppContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> GetCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer> GetCustomerById(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task<int> InsertCustomer(Customer customer)
        {
            try
            {
                await _context.Customers.AddAsync(customer);
                await _context.SaveChangesAsync();
                return customer.CustomerId;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new CustomException("Concurrency Error: An error occurred trying to save the data, please confirm the data and try again");
            }
            catch (DbUpdateException)
            {
                throw new CustomException("Update Error: An error occurred trying to save the data, please confirm the data and try again");
            }
        }

        public async Task<bool> UpdateCustomer(Customer customer)
        {
            try
            {
                _context.Customers.Update(customer);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new CustomException("Concurrency Error: An error occurred trying to save the data, please confirm the data and try again");
            }
            catch (DbUpdateException)
            {
                throw new CustomException("Update Error: An error occurred trying to save the data, please confirm the data and try again");
            }
        }

        public async Task<bool> DeleteCustomer(int id)
        {
            try
            {
                Customer customer = await _context.Customers.FindAsync(id);
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new CustomException("Concurrency Error: An error occurred trying to save the data, please confirm the data and try again");
            }
            catch (DbUpdateException)
            {
                throw new CustomException("Update Error: An error occurred trying to save the data, please confirm the data and try again");
            }
        }

        public async Task<bool> CustomerExists(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Entry(customer).State = EntityState.Detached;
                return true;
            }
            else
                return false;
        }

    }
}
