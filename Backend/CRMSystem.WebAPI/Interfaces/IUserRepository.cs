using CRMSystem.WebAPI.Models;

namespace CRMSystem.WebAPI.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User?> GetUserByIdAsync(Guid userId);
        Task<User> GetByUsernameAsync(string username);
        Task<bool> UsernameExistsAsync(string username);
        Task<bool> EmailExistsAsync(string email);
        Task UpdateAsync(User user);
        Task<bool> IsEmptyAsync();
    }
}