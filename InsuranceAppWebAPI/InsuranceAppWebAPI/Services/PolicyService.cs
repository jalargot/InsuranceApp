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
    public interface IPolicyService
    {
        Task<List<PolicyDTO>> GetPolicies();
        Task<PolicyDTO> GetPolicy(int id);
        Task<int> InsertPolicy(PolicyDTO policieDTO);
        Task<bool> UpdatePolicy(PolicyDTO policieDTO);
        Task<bool> DeletePolicy(int id);
        Task<bool> PolicyExists(int id);
    }

    public class PolicyService : IPolicyService
    {
        private readonly PolicyRepository _policyRepository;
        private readonly IMapper _mapper;

        public PolicyService(PolicyRepository policyRepository, IMapper mapper)
        {
            _policyRepository = policyRepository;
            _mapper = mapper;
        }

        public async Task<List<PolicyDTO>> GetPolicies()
        {
            var policies = await _policyRepository.GetPolicies();
            return _mapper.Map<List<PolicyDTO>>(policies);
        }

        public async Task<PolicyDTO> GetPolicy(int id)
        {
            var policie = await _policyRepository.GetPolicyById(id);
            if (policie == null)
                return null;
            return _mapper.Map<PolicyDTO>(policie);
        }

        public async Task<int> InsertPolicy(PolicyDTO policieDTO)
        {
            try
            {
                PolicyRule policyRule = new PolicyRule(policieDTO.Coverage, policieDTO.RiskType);
                var validation = policyRule.Validate();
                if (validation!=null)
                {
                    return 0;
                }
                var policie = _mapper.Map<Policy>(policieDTO);
                return await _policyRepository.InsertPolicy(policie);
            }
            catch (CustomException)
            {
                return 0;
            }
        }

        public async Task<bool> UpdatePolicy(PolicyDTO policieDTO)
        {
            try
            {
                PolicyRule policyRule = new PolicyRule(policieDTO.Coverage, policieDTO.RiskType);
                var validation = policyRule.Validate();
                if (validation != null)
                {
                    return false;
                }
                var policie = _mapper.Map<Policy>(policieDTO);
                return await _policyRepository.UpdatePolicy(policie);
            }
            catch (CustomException)
            {
                return false;
            }
        }

        public async Task<bool> DeletePolicy(int id)
        {
            try
            {
                return await _policyRepository.DeletePolicy(id);
            }
            catch (CustomException)
            {
                return false;
            }
        }

        public async Task<bool> PolicyExists(int id)
        {
            return await _policyRepository.PolicyExists(id);
        }
    }
}
