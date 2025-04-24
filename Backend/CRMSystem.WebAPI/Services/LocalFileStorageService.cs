using System.Security.Cryptography;
using CRMSystem.WebAPI.Interfaces;

namespace CRMSystem.WebAPI.Services
{
    public class LocalFileStorageService : IFileStorageService
    {
        public async Task<(string FileUrl, string FileHash)> SaveFileAsync(IFormFile file, HttpRequest request)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            Directory.CreateDirectory(uploadsFolder);

            using var sha256 = SHA256.Create();
            await using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            var fileBytes = memoryStream.ToArray();
            var fileHashBytes = sha256.ComputeHash(fileBytes);
            var fileHash = BitConverter.ToString(fileHashBytes).Replace("-", "").ToLower();

            var extension = Path.GetExtension(file.FileName);
            var newFileName = $"{fileHash}{extension}";
            var newFilePath = Path.Combine(uploadsFolder, newFileName);

            if (!File.Exists(newFilePath))
                await File.WriteAllBytesAsync(newFilePath, fileBytes);

            var fileUrl = $"{request.Scheme}://{request.Host}/uploads/{newFileName}";
            return (fileUrl, fileHash);
        }

        public async Task<string?> GetExistingFileUrlAsync(IFormFile file, HttpRequest request)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            Directory.CreateDirectory(uploadsFolder);
            
            using var sha256 = SHA256.Create();
            await using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            var fileHash = BitConverter
                .ToString(sha256.ComputeHash(memoryStream.ToArray()))
                .Replace("-", "").ToLower();

            var existingFilePath = Directory
                .EnumerateFiles(uploadsFolder)
                .FirstOrDefault(path => Path.GetFileNameWithoutExtension(path).StartsWith(fileHash));

            if (existingFilePath == null) return null;
            
            var existingFileName = Path.GetFileName(existingFilePath);
            return $"{request.Scheme}://{request.Host}/uploads/{existingFileName}";
        }
    }
}