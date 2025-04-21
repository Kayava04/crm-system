using AutoMapper;
using CRMSystem.WebAPI.Core;
using CRMSystem.WebAPI.Entities.Auth;
using CRMSystem.WebAPI.Interfaces;
using CRMSystem.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.WebAPI.Repositories
{
    public class UserRepository(SchoolDbContext context, IMapper mapper)
        : IUserRepository
    {
        public async Task AddAsync(User user)
        {
            var userEntity = new UserEntity
            {
                Id = user.Id,
                FullName = user.FullName,
                BirthDate = user.BirthDate,
                Email = user.Email,
                Username = user.Username,
                PasswordHash = user.PasswordHash,
                RoleId = user.RoleId,
                CreatedAt = user.CreatedAt
            };
            
            await context.Users.AddAsync(userEntity);
            await context.SaveChangesAsync();
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            var userEntity = await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
            
            return userEntity == null ? null : mapper.Map<User>(userEntity);
        }
        
        public async Task<User> GetByUsernameAsync(string username)
        {
            var userEntity = await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Username == username);
            
            return mapper.Map<User>(userEntity);
        }

        public async Task UpdateAsync(User user)
        {
            var existingEntity = await context.Users
                .FirstOrDefaultAsync(u => u.Id == user.Id);
            
            if (existingEntity == null)
                throw new InvalidOperationException("User not found");
            
            mapper.Map(user, existingEntity);
            await context.SaveChangesAsync();
        }
        
        public async Task<bool> IsEmptyAsync() => !await context.Users.AnyAsync();
    }
}