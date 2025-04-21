using System.Security.Cryptography;
using CRMSystem.WebAPI.Auth;
using CRMSystem.WebAPI.Interfaces;
using CRMSystem.WebAPI.Models;

namespace CRMSystem.WebAPI.Services
{
    public class UserService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider)
    {
        public async Task SignUp(string fullname, DateTime birthDate, string email, string username, string password)
        {
            var hashedPassword = passwordHasher.Generate(password);

            var isFirstUser = await userRepository.IsEmptyAsync();
            var roleId = isFirstUser ? 1 : 2;
        
            var user = User.Create(Guid.NewGuid(), fullname, birthDate, email, username, hashedPassword, roleId);
        
            await userRepository.AddAsync(user);
        }

        public async Task<string> SignIn(string username, string password)
        {
            var user = await userRepository.GetByUsernameAsync(username);
            var result = passwordHasher.Verify(password, user.PasswordHash);
        
            if (result == false)
                throw new InvalidOperationException("Invalid username or password");
        
            var token = jwtProvider.GenerateToken(user);
            return token;
        }

        public async Task<string> UploadUserPhoto(Guid userId, IFormFile file, HttpRequest request)
        {
            if (file.Length == 0)
                throw new InvalidOperationException("File is empty");
            
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            Directory.CreateDirectory(uploadsFolder);

            using var sha256 = SHA256.Create();
            await using var fileStream = file.OpenReadStream();
            var fileHashBytes = await sha256.ComputeHashAsync(fileStream);
            var fileHash = BitConverter.ToString(fileHashBytes).Replace("-", "").ToLower();
            
            var existingFilePath = Directory
                .EnumerateFiles(uploadsFolder)
                .FirstOrDefault(path => Path.GetFileNameWithoutExtension(path).StartsWith(fileHash));
            
            if (existingFilePath != null)
            {
                var existingFileName = Path.GetFileName(existingFilePath);
                var existingImageUrl = $"{request.Scheme}://{request.Host}/uploads/{existingFileName}";

                var user = await userRepository.GetUserByIdAsync(userId);
                
                if (user == null)
                    throw new InvalidOperationException("User not found");

                if (user.ImageUrl != existingImageUrl)
                    user.SetImageUrl(existingImageUrl);

                user.TouchUpdatedAt();
                await userRepository.UpdateAsync(user);
                
                return existingImageUrl;
            }
            
            var extension = Path.GetExtension(file.FileName);
            var newFileName = $"{fileHash}{extension}";
            var newFilePath = Path.Combine(uploadsFolder, newFileName);

            fileStream.Position = 0;
            
            await using var saveStream = new FileStream(newFilePath, FileMode.Create);
            await fileStream.CopyToAsync(saveStream);

            var imageUrl = $"{request.Scheme}://{request.Host}/uploads/{newFileName}";

            var userToUpdate = await userRepository.GetUserByIdAsync(userId);
            
            if (userToUpdate == null)
                throw new InvalidOperationException("User not found");

            userToUpdate.SetImageUrl(imageUrl);
            userToUpdate.TouchUpdatedAt();
            await userRepository.UpdateAsync(userToUpdate);

            return imageUrl;
        }

        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            var user = await userRepository.GetUserByIdAsync(userId);
            
            if (user == null)
                throw new InvalidOperationException("User not found");
            
            return user;
        }

        public async Task UpdateUserAsync(Guid userId, string fullName, DateTime birthDate, string email,
            string? oldPassword, string? newPassword, string? confirmNewPassword)
        {
            var user = await userRepository.GetUserByIdAsync(userId);
            
            if (user == null)
                throw new InvalidOperationException("User not found");
            
            string? newPasswordHash = null;

            var passwordChangeRequested = 
                !string.IsNullOrWhiteSpace(oldPassword) || 
                !string.IsNullOrWhiteSpace(newPassword) || 
                !string.IsNullOrWhiteSpace(confirmNewPassword);
            
            if (passwordChangeRequested)
            {
                if (string.IsNullOrWhiteSpace(oldPassword) || !passwordHasher.Verify(oldPassword, user.PasswordHash))
                    throw new InvalidOperationException("Old password is incorrect!");

                if (string.IsNullOrWhiteSpace(newPassword) || newPassword != confirmNewPassword)
                    throw new InvalidOperationException("New password is invalid or does not match!");

                newPasswordHash = passwordHasher.Generate(newPassword);
            }
            
            user.Update(fullName, birthDate, email, newPasswordHash);
            await userRepository.UpdateAsync(user);
        }
    }
}