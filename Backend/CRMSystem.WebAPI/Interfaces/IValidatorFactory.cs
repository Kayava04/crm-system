namespace CRMSystem.WebAPI.Interfaces
{
    public interface IValidatorFactory
    {
        IRequestValidator GetValidator(object type);
        bool Validate(object dto, out string message);
    }
}