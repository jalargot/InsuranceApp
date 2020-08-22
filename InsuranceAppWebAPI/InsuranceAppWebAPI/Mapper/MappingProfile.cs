using AutoMapper;
using InsuranceAppWebAPI.DTOs;
using InsuranceAppWebAPI.Models;
using System.Diagnostics.CodeAnalysis;

namespace InsuranceAppWebAPI.Mapper
{
    [ExcludeFromCodeCoverage]
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            CreateMap<Customer, CustomerDTO>().ReverseMap();
            CreateMap<Policy, PolicyDTO>().ReverseMap();
        }
    }
}
