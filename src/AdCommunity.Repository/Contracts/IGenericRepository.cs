using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace AdCommunity.Repository.Contracts;

public interface IGenericRepository<T> where T : class
{
    Task<T> GetById(int id);
    Task<IEnumerable<T>> GetAll();
    Task Add(T entity);
    void Delete(T entity);
    void Update(T entity);
}