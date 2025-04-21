namespace CRMSystem.WebAPI.Interfaces
{
    public interface IBasicRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync(IFilterDto? filter = null);
        Task<T> AddAsync(T entity);
        Task<T?> UpdateAsync(Guid id, T entity);
        Task DeleteAsync(Guid id);
    }
}