namespace CRMSystem.WebAPI.Interfaces
{
    public interface IUploadService
    {
        Task<string> UploadUserPhoto(Guid userId, IFormFile file, HttpRequest request);
    }
}