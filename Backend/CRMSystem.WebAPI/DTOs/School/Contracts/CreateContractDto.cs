namespace CRMSystem.WebAPI.DTOs.School.Contracts
{
    public record CreateContractDto(string ContractNumber, DateTime SignDate, decimal PaymentAmount, Guid StudentId);
}