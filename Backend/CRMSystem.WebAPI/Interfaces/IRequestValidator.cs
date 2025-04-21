namespace CRMSystem.WebAPI.Interfaces
{
    public interface IRequestValidator
    {
        bool IsValid(object dto, out string errorMessage);
    }
}