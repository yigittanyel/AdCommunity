using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace AdCommunity.Repository.Contracts;

public interface IGenericRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    void Delete(T entity);
    void Update(T entity);
}