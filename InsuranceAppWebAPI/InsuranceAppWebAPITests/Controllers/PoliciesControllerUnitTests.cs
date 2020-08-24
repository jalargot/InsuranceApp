using NUnit.Framework;
using InsuranceAppWebAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using InsuranceAppWebAPI.Services;
using InsuranceAppWebAPI.DTOs;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using FluentAssertions.Execution;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace InsuranceAppWebAPI.Controllers.Tests
{
    [TestFixture()]
    public class PoliciesControllerUnitTests
    {
        private PolicyDTO policy;
        private List<PolicyDTO> policies = new List<PolicyDTO>();

        [SetUp]
        public void SetUp()
        {
            policy = new PolicyDTO
            {
                PolicyId = 1
            };

            policies.Add(policy);
        }

        [Test()]
        public async Task GetUsers_ReturnsOK()
        {
            var mockService = new Mock<IPolicyService>();
            mockService.Setup(p => p.GetPolicies()).ReturnsAsync(policies);
            var mockController = new PoliciesController(mockService.Object);
            var response = await mockController.GetPolicies();
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
        public async Task GetPolicy_ValidId_ReturnsOk()
        {
            var mockService = new Mock<IPolicyService>();
            mockService.Setup(p => p.GetPolicy(policy.PolicyId)).ReturnsAsync(policy);
            var mockController = new PoliciesController(mockService.Object);
            var response = await mockController.GetPolicy(policy.PolicyId);
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
        public async Task GetPolicy_InvalidId_ReturnsNotFound()
        {
            var mockService = new Mock<IPolicyService>();
            mockService.Setup(p => p.GetPolicy(policy.PolicyId)).ReturnsAsync(() => null);
            var mockController = new PoliciesController(mockService.Object);
            var response = await mockController.GetPolicy(policy.PolicyId);
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
        public async Task PutPolicy_ValidIdAndObject_ReturnsOK()
        {
            var mockService = new Mock<IPolicyService>();
            mockService.Setup(p => p.PolicyExists(policy.PolicyId)).ReturnsAsync(true);
            mockService.Setup(p => p.UpdatePolicy(policy)).ReturnsAsync(true);
            var mockController = new PoliciesController(mockService.Object);
            var response = await mockController.PutPolicy(policy.PolicyId, policy);
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
        public async Task PutPolicy_InvalidRequest_ReturnsBadRequest()
        {
            var mockService = new Mock<IPolicyService>();
            var mockController = new PoliciesController(mockService.Object);
            var response = await mockController.PutPolicy(0, policy);
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
        public async Task PutPolicy_InvalidId_ReturnsNotFound()
        {
            var mockService = new Mock<IPolicyService>();
            mockService.Setup(p => p.PolicyExists(policy.PolicyId)).ReturnsAsync(false);
            var mockController = new PoliciesController(mockService.Object);
            var response = await mockController.PutPolicy(policy.PolicyId, policy);
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
        public async Task PutPolicy_ValidIdAndInvalidObject_ReturnsConflict()
        {
            var mockService = new Mock<IPolicyService>();
            mockService.Setup(p => p.PolicyExists(policy.PolicyId)).ReturnsAsync(true);
            mockService.Setup(p => p.UpdatePolicy(policy)).ReturnsAsync(false);
            var mockController = new PoliciesController(mockService.Object);
            var response = await mockController.PutPolicy(policy.PolicyId, policy);
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
        public async Task PostPolicy_ValidObject_ReturnsCreatedAt()
        {
            var mockService = new Mock<IPolicyService>();
            mockService.Setup(p => p.InsertPolicy(policy)).ReturnsAsync(policy.PolicyId);
            var mockController = new PoliciesController(mockService.Object);
            var response = await mockController.PostPolicy(policy);
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
        public async Task PostPolicy_InvalidObject_ReturnsConflict()
        {
            var mockService = new Mock<IPolicyService>();
            mockService.Setup(p => p.InsertPolicy(policy)).ReturnsAsync(0);
            var mockController = new PoliciesController(mockService.Object);
            var response = await mockController.PostPolicy(policy);
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
        public async Task DeletePolicy_ValidId_ReturnsOK()
        {
            var mockService = new Mock<IPolicyService>();
            mockService.Setup(p => p.PolicyExists(policy.PolicyId)).ReturnsAsync(true);
            mockService.Setup(p => p.DeletePolicy(policy.PolicyId)).ReturnsAsync(true);
            var mockController = new PoliciesController(mockService.Object);
            var response = await mockController.DeletePolicy(policy.PolicyId);
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
        public async Task DeletePolicy__InvalidId_ReturnsNotFound()
        {
            var mockService = new Mock<IPolicyService>();
            mockService.Setup(p => p.PolicyExists(policy.PolicyId)).ReturnsAsync(false);
            var mockController = new PoliciesController(mockService.Object);
            var response = await mockController.DeletePolicy(policy.PolicyId);
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
        public async Task DeletePolicy_Exception_ReturnsConflict()
        {
            var mockService = new Mock<IPolicyService>();
            mockService.Setup(p => p.PolicyExists(policy.PolicyId)).ReturnsAsync(true);
            mockService.Setup(p => p.DeletePolicy(policy.PolicyId)).ReturnsAsync(false);
            var mockController = new PoliciesController(mockService.Object);
            var response = await mockController.DeletePolicy(policy.PolicyId);
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