namespace CRMSystem.WebAPI.Interfaces
{
    public interface IContractNumberGenerator
    {
        Task<string> Generate(DateTime signDate);
    }
}