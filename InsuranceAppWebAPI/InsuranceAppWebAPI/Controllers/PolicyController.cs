using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using InsuranceAppWebAPI.DTOs;
using InsuranceAppWebAPI.Exceptions;
using InsuranceAppWebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceAppWebAPI.Controllers
{
    [Route("api/policies")]
    [ApiController]
    public class PoliciesController : ControllerBase
    {
        private readonly IPolicyService _policyService;

        public PoliciesController(IPolicyService policyService)
        {
            _policyService = policyService;
        }

        /// <summary>
        /// Returns a List of all policies
        /// </summary>
        /// <returns>List of all Policies</returns>
        /// <response code="200">Returns a List of all policies</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PolicyDTO>>> GetPolicies()
        {
            return Ok(await _policyService.GetPolicies());
        }

        /// <summary>
        /// Returns an policy for a given id
        /// </summary>
        /// <param name="id">Policy Id</param>
        /// <returns>Policy for especific Id</returns>
        /// <response code="200">Returns an policy for a given id</response>
        /// <response code="404">If the policy is not found</response>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PolicyDTO>> GetPolicy(int id)
        {
            var policyDTO = await _policyService.GetPolicy(id);

            if (policyDTO == null)
            {
                return NotFound();
            }

            return Ok(policyDTO);
        }

        /// <summary>
        /// update an policy for a given id
        /// </summary>
        /// <param name="id">Policy Id</param>
        /// <param name="policyDTO">Policy object to update</param>
        /// <returns>Empty Ok response</returns>
        /// <response code="200">Empty Ok response</response>
        /// <response code="400">If the id is different to the policyId</response>
        /// <response code="404">If the policy is not found</response>
        /// <response code="409">If there is a problem updating the policy</response>
        /// <response code="422">If the entity is breaking the business rules</response> 
        [HttpPut("{id}")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult> PutPolicy(int id, PolicyDTO policyDTO)
        {
            try
            {
                // According to the HTTP specification, a PUT request requires
                // the client to send the entire updated entity, not just the changes.
                // To support partial updates, implements HTTP PATCH

                if (id != policyDTO.PolicyId)
                {
                    return BadRequest();
                }

                var policyExists = await _policyService.PolicyExists(id);

                if (!policyExists)
                {
                    return NotFound();
                }

                var status = await _policyService.UpdatePolicy(policyDTO);
                if (!status)
                {
                    return Conflict();
                }

                return Ok();
            }
            catch (BusinessRuleException)
            {
                return UnprocessableEntity();
            }
            
        }

        /// <summary>
        /// add a new policy
        /// </summary>
        /// <param name="policyDTO">Policy to create</param>
        /// <returns>A newly created policy</returns>
        /// <response code="201">Returns the newly created policy</response>
        /// <response code="409">If there is a problem updating the policy</response> 
        /// <response code="422">If the entity is breaking the business rules</response> 
        [HttpPost]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<PolicyDTO>> PostPolicy(PolicyDTO policyDTO)
        {
            try
            {
                var id = await _policyService.InsertPolicy(policyDTO);
                if (id == 0)
                {
                    return Conflict();
                }

                return CreatedAtAction(nameof(GetPolicy), new { id }, policyDTO);
            }
            catch (BusinessRuleException)
            {
                return UnprocessableEntity();
            }
            
        }

        /// <summary>
        /// delete an policy for a given id
        /// </summary>
        /// <param name="id">Policy id</param>
        /// <returns>Empty Ok response</returns>
        /// <response code="200">Empty Ok response</response>
        /// <response code="404">If the policy is not found</response>
        /// <response code="409">If there is a problem updating the policy</response>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeletePolicy(int id)
        {
            var policyExists = await _policyService.PolicyExists(id);

            if (!policyExists)
            {
                return NotFound();
            }

            var status = await _policyService.DeletePolicy(id);
            if (!status)
            {
                return Conflict();
            }

            return Ok();
        }

    }
}
