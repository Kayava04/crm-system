namespace CRMSystem.WebAPI.DTOs.School.Contracts
{
    public record ContractDto(Guid Id, string ContractNumber, DateTime SignDate, decimal PaymentAmount, Guid StudentId);
}