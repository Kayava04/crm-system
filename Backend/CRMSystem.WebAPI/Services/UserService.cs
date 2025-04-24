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
            if (await userRepository.EmailExistsAsync(email))
                throw new InvalidOperationException("This email is already in use.");

            if (await userRepository.UsernameExistsAsync(username))
                throw new InvalidOperationException("A user with this username already exists.");
            
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
        
        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            var user = await userRepository.GetUserByIdAsync(userId);
            
            if (user == null)
                throw new InvalidOperationException("User not found");
            
            return user;
        }
        
        public async Task UpdateUserAsync(Guid userId, string? fullName, DateTime? birthDate, string? email)
        {
            var user = await userRepository.GetUserByIdAsync(userId);
            
            if (user == null)
                throw new InvalidOperationException("User not found");
            
            if (!string.IsNullOrWhiteSpace(email) &&
                !email.Equals(user.Email, StringComparison.OrdinalIgnoreCase) &&
                await userRepository.EmailExistsAsync(email))
            {
                throw new InvalidOperationException("A user with this email already exists.");
            }
            
            user.Update(fullName, birthDate, email);
            user.TouchUpdatedAt();
            
            await userRepository.UpdateAsync(user);
        }

        public async Task UpdateUserPasswordAsync(Guid userId, string oldPassword, string newPassword,
            string confirmNewPassword)
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
            
            user.UpdatePassword(newPasswordHash);
            user.TouchUpdatedAt();
            
            await userRepository.UpdateAsync(user);
        }
    }
}