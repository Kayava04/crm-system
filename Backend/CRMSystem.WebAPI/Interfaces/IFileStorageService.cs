namespace CRMSystem.WebAPI.Interfaces
{
    public interface IFileStorageService
    {
        Task<(string FileUrl, string FileHash)> SaveFileAsync(IFormFile file, HttpRequest request);
        Task<string?> GetExistingFileUrlAsync(IFormFile file, HttpRequest request);
    }
}