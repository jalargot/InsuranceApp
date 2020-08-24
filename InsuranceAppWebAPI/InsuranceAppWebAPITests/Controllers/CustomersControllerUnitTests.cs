using NUnit.Framework;
using InsuranceAppWebAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Moq;
using InsuranceAppWebAPI.Services;
using Newtonsoft.Json;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using FluentAssertions;
using InsuranceAppWebAPI.DTOs;

namespace InsuranceAppWebAPI.Controllers.Tests
{
    [TestFixture()]
    public class CustomersControllerUnitTests
    {
        private CustomerDTO customer;
        private List<CustomerDTO> customers = new List<CustomerDTO>();

        [SetUp]
        public void SetUp()
        {
            customer = new CustomerDTO
            {
                CustomerId = 1
            };

            customers.Add(customer);
        }

        [Test()]
        public async Task GetUsers_ReturnsOK()
        {
            var mockService = new Mock<ICustomerService>();
            mockService.Setup(p => p.GetCustomers()).ReturnsAsync(customers);
            var mockController = new CustomersController(mockService.Object);
            var response = await mockController.GetCustomers();
            TestContext.WriteLine(JsonConvert.SerializeObject(response));

            using (new AssertionScope())
            {
                response.Should().NotBeNull();
                response.Result.Should().NotBeNull().And.BeOfType<OkObjectResult>();
                var okResponse = (OkObjectResult)response.Result;
                okResponse.Should().NotBeNull().And.BeOfType<OkObjectResult>();
                okResponse.StatusCode.Should().NotBeNull().And.Be(StatusCodes.Status200OK);
            }
        }

        [Test()]
        public async Task GetCustomer_ValidId_ReturnsOk()
        {
            var mockService = new Mock<ICustomerService>();
            mockService.Setup(p => p.GetCustomer(customer.CustomerId)).ReturnsAsync(customer);
            var mockController = new CustomersController(mockService.Object);
            var response = await mockController.GetCustomer(customer.CustomerId);
            TestContext.WriteLine(JsonConvert.SerializeObject(response));

            using (new AssertionScope())
            {
                response.Should().NotBeNull();
                response.Result.Should().NotBeNull().And.BeOfType<OkObjectResult>();
                var okResponse = (OkObjectResult)response.Result;
                okResponse.Should().NotBeNull().And.BeOfType<OkObjectResult>();
                okResponse.StatusCode.Should().NotBeNull().And.Be(StatusCodes.Status200OK);
            }
        }

        [Test()]
        public async Task GetCustomer_InvalidId_ReturnsNotFound()
        {
            var mockService = new Mock<ICustomerService>();
            mockService.Setup(p => p.GetCustomer(customer.CustomerId)).ReturnsAsync(() => null);
            var mockController = new CustomersController(mockService.Object);
            var response = await mockController.GetCustomer(customer.CustomerId);
            TestContext.WriteLine(JsonConvert.SerializeObject(response));

            using (new AssertionScope())
            {
                response.Should().NotBeNull();
                response.Result.Should().NotBeNull().And.BeOfType<NotFoundResult>();
                var notFoundResponse = (NotFoundResult)response.Result;
                notFoundResponse.Should().NotBeNull().And.BeOfType<NotFoundResult>();
                notFoundResponse.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            }
        }

        [Test()]
        public async Task PutCustomer_ValidIdAndObject_ReturnsOK()
        {
            var mockService = new Mock<ICustomerService>();
            mockService.Setup(p => p.CustomerExists(customer.CustomerId)).ReturnsAsync(true);
            mockService.Setup(p => p.UpdateCustomer(customer)).ReturnsAsync(true);
            var mockController = new CustomersController(mockService.Object);
            var response = await mockController.PutCustomer(customer.CustomerId, customer);
            TestContext.WriteLine(JsonConvert.SerializeObject(response));

            using (new AssertionScope())
            {
                response.Should().NotBeNull().And.BeOfType<OkResult>();
                var okResponse = (OkResult)response;
                okResponse.Should().NotBeNull().And.BeOfType<OkResult>();
                okResponse.StatusCode.Should().Be(StatusCodes.Status200OK);
            }
        }

        [Test()]
        public async Task PutCustomer_InvalidRequest_ReturnsBadRequest()
        {
            var mockService = new Mock<ICustomerService>();
            var mockController = new CustomersController(mockService.Object);
            var response = await mockController.PutCustomer(0, customer);
            TestContext.WriteLine(JsonConvert.SerializeObject(response));

            using (new AssertionScope())
            {
                response.Should().NotBeNull().And.BeOfType<BadRequestResult>();
                var badRequestResponse = (BadRequestResult)response;
                badRequestResponse.Should().NotBeNull().And.BeOfType<BadRequestResult>();
                badRequestResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            }
        }

        [Test()]
        public async Task PutCustomer_InvalidId_ReturnsNotFound()
        {
            var mockService = new Mock<ICustomerService>();
            mockService.Setup(p => p.CustomerExists(customer.CustomerId)).ReturnsAsync(false);
            var mockController = new CustomersController(mockService.Object);
            var response = await mockController.PutCustomer(customer.CustomerId, customer);
            TestContext.WriteLine(JsonConvert.SerializeObject(response));

            using (new AssertionScope())
            {
                response.Should().NotBeNull().And.BeOfType<NotFoundResult>();
                var notFoundResponse = (NotFoundResult)response;
                notFoundResponse.Should().NotBeNull().And.BeOfType<NotFoundResult>();
                notFoundResponse.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            }
        }

        [Test()]
        public async Task PutCustomer_ValidIdAndInvalidObject_ReturnsConflict()
        {
            var mockService = new Mock<ICustomerService>();
            mockService.Setup(p => p.CustomerExists(customer.CustomerId)).ReturnsAsync(true);
            mockService.Setup(p => p.UpdateCustomer(customer)).ReturnsAsync(false);
            var mockController = new CustomersController(mockService.Object);
            var response = await mockController.PutCustomer(customer.CustomerId,customer);
            TestContext.WriteLine(JsonConvert.SerializeObject(response));

            using (new AssertionScope())
            {
                response.Should().NotBeNull().And.BeOfType<ConflictResult>();
                var conflictResponse = (ConflictResult)response;
                conflictResponse.Should().NotBeNull().And.BeOfType<ConflictResult>();
                conflictResponse.StatusCode.Should().Be(StatusCodes.Status409Conflict);
            }
        }

        [Test()]
        public async Task PostCustomer_ValidObject_ReturnsCreatedAt()
        {
            var mockService = new Mock<ICustomerService>();
            mockService.Setup(p => p.InsertCustomer(customer)).ReturnsAsync(customer.CustomerId);
            var mockController = new CustomersController(mockService.Object);
            var response = await mockController.PostCustomer(customer);
            TestContext.WriteLine(JsonConvert.SerializeObject(response));

            using (new AssertionScope())
            {
                response.Should().NotBeNull();
                response.Result.Should().NotBeNull().And.BeOfType<CreatedAtActionResult>();
                var createdAtResponse = (CreatedAtActionResult)response.Result;
                createdAtResponse.Should().NotBeNull().And.BeOfType<CreatedAtActionResult>();
                createdAtResponse.StatusCode.Should().NotBeNull().And.Be(StatusCodes.Status201Created);
            }
        }

        [Test()]
        public async Task PostCustomer_InvalidObject_ReturnsConflict()
        {
            var mockService = new Mock<ICustomerService>();
            mockService.Setup(p => p.InsertCustomer(customer)).ReturnsAsync(0);
            var mockController = new CustomersController(mockService.Object);
            var response = await mockController.PostCustomer(customer);
            TestContext.WriteLine(JsonConvert.SerializeObject(response));
           
            using (new AssertionScope())
            {
                response.Should().NotBeNull();
                response.Result.Should().NotBeNull().And.BeOfType<ConflictResult>();
                var conflictResponse = (ConflictResult)response.Result;
                conflictResponse.Should().NotBeNull().And.BeOfType<ConflictResult>();
                conflictResponse.StatusCode.Should().Be(StatusCodes.Status409Conflict);
            }
        }

        [Test()]
        public async Task DeleteCustomer_ValidId_ReturnsOK()
        {
            var mockService = new Mock<ICustomerService>();
            mockService.Setup(p => p.CustomerExists(customer.CustomerId)).ReturnsAsync(true);
            mockService.Setup(p => p.DeleteCustomer(customer.CustomerId)).ReturnsAsync(true);
            var mockController = new CustomersController(mockService.Object);
            var response = await mockController.DeleteCustomer(customer.CustomerId);
            TestContext.WriteLine(JsonConvert.SerializeObject(response));

            using (new AssertionScope())
            {
                response.Should().NotBeNull().And.BeOfType<OkResult>();
                var okResponse = (OkResult)response;
                okResponse.Should().NotBeNull().And.BeOfType<OkResult>();
                okResponse.StatusCode.Should().Be(StatusCodes.Status200OK);
            }
        }

        [Test()]
        public async Task DeleteCustomer__InvalidId_ReturnsNotFound()
        {
            var mockService = new Mock<ICustomerService>();
            mockService.Setup(p => p.CustomerExists(customer.CustomerId)).ReturnsAsync(false);
            var mockController = new CustomersController(mockService.Object);
            var response = await mockController.DeleteCustomer(customer.CustomerId);
            TestContext.WriteLine(JsonConvert.SerializeObject(response));

            using (new AssertionScope())
            {
                response.Should().NotBeNull().And.BeOfType<NotFoundResult>();
                var notFoundResponse = (NotFoundResult)response;
                notFoundResponse.Should().NotBeNull().And.BeOfType<NotFoundResult>();
                notFoundResponse.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            }
        }

        [Test()]
        public async Task DeleteCustomer_Exception_ReturnsConflict()
        {
            var mockService = new Mock<ICustomerService>();
            mockService.Setup(p => p.CustomerExists(customer.CustomerId)).ReturnsAsync(true);
            mockService.Setup(p => p.DeleteCustomer(customer.CustomerId)).ReturnsAsync(false);
            var mockController = new CustomersController(mockService.Object);
            var response = await mockController.DeleteCustomer(customer.CustomerId);
            TestContext.WriteLine(JsonConvert.SerializeObject(response));

            using (new AssertionScope())
            {
                response.Should().NotBeNull().And.BeOfType<ConflictResult>();
                var conflictResponse = (ConflictResult)response;
                conflictResponse.Should().NotBeNull().And.BeOfType<ConflictResult>();
                conflictResponse.StatusCode.Should().Be(StatusCodes.Status409Conflict);
            }
        }
    }
}