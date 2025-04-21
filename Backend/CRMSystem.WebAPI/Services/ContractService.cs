using AutoMapper;
using CRMSystem.WebAPI.DTOs.School.Contracts;
using CRMSystem.WebAPI.Interfaces;
using CRMSystem.WebAPI.Models;

namespace CRMSystem.WebAPI.Services
{
    public class ContractService(
        IBasicRepository<Contract> contractRepository,
        IContractNumberGenerator contractNumberGenerator,
        IMapper mapper)
    {
        public async Task<IEnumerable<ContractDto>> GetAllAsync()
        {
            var contracts = await contractRepository.GetAllAsync();
            
            return mapper.Map<IEnumerable<ContractDto>>(contracts);
        }

        public async Task<ContractDto?> GetByIdAsync(Guid id)
        {
            var contract = await contractRepository.GetByIdAsync(id);
            
            return contract is null ? null : mapper.Map<ContractDto>(contract);
        }

        public async Task<ContractDto> CreateAsync(CreateContractDto dto)
        {
            var contractNumber = await contractNumberGenerator.Generate(dto.SignDate);
            
            var contract = Contract.Create(Guid.NewGuid(), contractNumber, dto.SignDate, dto.PaymentAmount, dto.StudentId);
            var created = await contractRepository.AddAsync(contract);
            
            return mapper.Map<ContractDto>(created);
        }

        public async Task<ContractDto?> UpdateAsync(Guid id, CreateContractDto dto)
        {
            var contract = Contract.Create(id, dto.ContractNumber, dto.SignDate, dto.PaymentAmount, dto.StudentId);
            var updated = await contractRepository.UpdateAsync(id, contract);
            
            return updated is null ? null : mapper.Map<ContractDto>(updated);
        }

        public async Task DeleteAsync(Guid id)
        {
            await contractRepository.DeleteAsync(id);
        }
    }
}