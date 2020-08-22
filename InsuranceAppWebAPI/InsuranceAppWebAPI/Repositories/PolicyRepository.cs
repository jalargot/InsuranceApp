using InsuranceAppWebAPI.Contexts;
using InsuranceAppWebAPI.Exceptions;
using InsuranceAppWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InsuranceAppWebAPI.Repositories
{
    public class PolicyRepository
    {
        private readonly InsuranceAppContext _context;

        public PolicyRepository(InsuranceAppContext context)
        {
            _context = context;
        }

        public async Task<List<Policy>> GetPolicies()
        {
            return await _context.Policies.ToListAsync();
        }

        public async Task<Policy> GetPolicyById(int id)
        {
            return await _context.Policies.FindAsync(id);
        }

        public async Task<int> InsertPolicy(Policy policy)
        {
            try
            {
                await _context.Policies.AddAsync(policy);
                await _context.SaveChangesAsync();
                return policy.PolicyId;
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

        public async Task<bool> UpdatePolicy(Policy policy)
        {
            try
            {
                _context.Policies.Update(policy);
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

        public async Task<bool> DeletePolicy(int id)
        {
            try
            {
                Policy policy = await _context.Policies.FindAsync(id);
                _context.Policies.Remove(policy);
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

        public async Task<bool> PolicyExists(int id)
        {
            var policy = await _context.Policies.FindAsync(id);
            if (policy != null)
            {
                _context.Entry(policy).State = EntityState.Detached;
                return true;
            }
            else
                return false;
        }

    }
}
