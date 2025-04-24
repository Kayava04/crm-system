using CRMSystem.WebAPI.Interfaces;

namespace CRMSystem.WebAPI.Services
{
    public class UploadService(
        IUserRepository userRepository,
        IFileStorageService fileStorageService) : IUploadService
    {
        public async Task<string> UploadUserPhoto(Guid userId, IFormFile file, HttpRequest request)
        {
            if (file.Length == 0)
                throw new InvalidOperationException("File is empty");

            var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif" };
            
            if (!allowedTypes.Contains(file.ContentType))
                throw new InvalidOperationException("Unsupported file type");

            var existingFileUrl = await fileStorageService.GetExistingFileUrlAsync(file, request);
            
            var user = await userRepository.GetUserByIdAsync(userId)
                ?? throw new InvalidOperationException("User not found");

            if (existingFileUrl != null)
            {
                if (user.ImageUrl != existingFileUrl)
                    user.SetImageUrl(existingFileUrl);

                user.TouchUpdatedAt();
                await userRepository.UpdateAsync(user);
                
                return existingFileUrl;
            }

            var (imageUrl, _) = await fileStorageService.SaveFileAsync(file, request);
            user.SetImageUrl(imageUrl);
            user.TouchUpdatedAt();
            await userRepository.UpdateAsync(user);

            return imageUrl;
        }
    }
}