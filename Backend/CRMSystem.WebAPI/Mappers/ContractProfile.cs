using AutoMapper;
using CRMSystem.WebAPI.DTOs.School.Contracts;
using CRMSystem.WebAPI.Entities.School;
using CRMSystem.WebAPI.Models;

namespace CRMSystem.WebAPI.Mappers
{
    public class ContractProfile : Profile
    {
        public ContractProfile()
        {
            CreateMap<ContractEntity, Contract>().ConstructUsing(e =>
                Contract.Create(e.Id, e.ContractNumber, e.SignDate, e.PaymentAmount, e.StudentId));
            
            CreateMap<Contract, ContractEntity>();
            CreateMap<Contract, ContractDto>();
            CreateMap<CreateContractDto, Contract>();
        }
    }
}