using AutoMapper;
using InsuranceAppWebAPI.DTOs;
using InsuranceAppWebAPI.Exceptions;
using InsuranceAppWebAPI.Models;
using InsuranceAppWebAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceAppWebAPI.Services
{
    public interface ICustomerService
    {
        Task<List<CustomerDTO>> GetCustomers();
        Task<CustomerDTO> GetCustomer(int id);
        Task<int> InsertCustomer(CustomerDTO customerDTO);
        Task<bool> UpdateCustomer(CustomerDTO customerDTO);
        Task<bool> DeleteCustomer(int id);
        Task<bool> CustomerExists(int id);
    }

    public class CustomerService : ICustomerService
    {
        private readonly CustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(CustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<List<CustomerDTO>> GetCustomers()
        {
            var customers = await _customerRepository.GetCustomers();
            return _mapper.Map<List<CustomerDTO>>(customers);
        }

        public async Task<CustomerDTO> GetCustomer(int id)
        {
            var customer = await _customerRepository.GetCustomerById(id);
            if (customer == null)
                return null;
            return _mapper.Map<CustomerDTO>(customer);
        }

        public async Task<int> InsertCustomer(CustomerDTO customerDTO)
        {
            try
            {
                var customer = _mapper.Map<Customer>(customerDTO);
                return await _customerRepository.InsertCustomer(customer);
            }
            catch (CustomException)
            {
                return 0;
            }
        }

        public async Task<bool> UpdateCustomer(CustomerDTO customerDTO)
        {
            try
            {
                var customer = _mapper.Map<Customer>(customerDTO);
                return await _customerRepository.UpdateCustomer(customer);
            }
            catch (CustomException)
            {
                return false;
            }
        }

        public async Task<bool> DeleteCustomer(int id)
        {
            try
            {
                return await _customerRepository.DeleteCustomer(id);
            }
            catch (CustomException)
            {
                return false;
            }
        }

        public async Task<bool> CustomerExists(int id)
        {
            return await _customerRepository.CustomerExists(id);
        }
    }
}
