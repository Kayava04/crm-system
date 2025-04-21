using CRMSystem.WebAPI.Models;

namespace CRMSystem.WebAPI.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User?> GetUserByIdAsync(Guid userId);
        Task<User> GetByUsernameAsync(string username);
        Task UpdateAsync(User user);
        Task<bool> IsEmptyAsync();
    }
}