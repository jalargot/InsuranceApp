using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using InsuranceAppWebAPI.DTOs;
using InsuranceAppWebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceAppWebAPI.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// Returns a List of all customers
        /// </summary>
        /// <returns>List of all Customers</returns>
        /// <response code="200">Returns a List of all customers</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetCustomers()
        {
            return Ok(await _customerService.GetCustomers());
        }

        /// <summary>
        /// Returns an customer for a given id
        /// </summary>
        /// <param name="id">Customer Id</param>
        /// <returns>Customer for especific Id</returns>
        /// <response code="200">Returns an customer for a given id</response>
        /// <response code="404">If the customer is not found</response>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CustomerDTO>> GetCustomer(int id)
        {
            var customerDTO = await _customerService.GetCustomer(id);

            if (customerDTO == null)
            {
                return NotFound();
            }

            return Ok(customerDTO);
        }

        /// <summary>
        /// update an customer for a given id
        /// </summary>
        /// <param name="id">Customer Id</param>
        /// <param name="customerDTO">Customer object to update</param>
        /// <returns>Empty Ok response</returns>
        /// <response code="200">Empty Ok response</response>
        /// <response code="400">If the id is different to the customerId</response>
        /// <response code="404">If the customer is not found</response>
        /// <response code="409">If there is a problem updating the customer</response>
        [HttpPut("{id}")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> PutCustomer(int id, CustomerDTO customerDTO)
        {
            // According to the HTTP specification, a PUT request requires
            // the client to send the entire updated entity, not just the changes.
            // To support partial updates, implements HTTP PATCH

            if (id != customerDTO.CustomerId)
            {
                return BadRequest();
            }

            var customerExists = await _customerService.CustomerExists(id);

            if (!customerExists)
            {
                return NotFound();
            }

            var status = await _customerService.UpdateCustomer(customerDTO);
            if (!status)
            {
                return Conflict();
            }

            return Ok();
        }

        /// <summary>
        /// add a new customer
        /// </summary>
        /// <param name="customerDTO">Customer to create</param>
        /// <returns>A newly created customer</returns>
        /// <response code="201">Returns the newly created customer</response>
        /// <response code="409">If there is a problem updating the customer</response> 
        [HttpPost]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<CustomerDTO>> PostCustomer(CustomerDTO customerDTO)
        {
            var id = await _customerService.InsertCustomer(customerDTO);
            if (id == 0)
            {
                return Conflict();
            }

            return CreatedAtAction(nameof(GetCustomer), new { id }, customerDTO);
        }

        /// <summary>
        /// delete an customer for a given id
        /// </summary>
        /// <param name="id">Customer id</param>
        /// <returns>Empty Ok response</returns>
        /// <response code="200">Empty Ok response</response>
        /// <response code="404">If the customer is not found</response>
        /// <response code="409">If there is a problem updating the customer</response>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            var customerExists = await _customerService.CustomerExists(id);

            if (!customerExists)
            {
                return NotFound();
            }

            var status = await _customerService.DeleteCustomer(id);
            if (!status)
            {
                return Conflict();
            }

            return Ok();
        }

    }
}
