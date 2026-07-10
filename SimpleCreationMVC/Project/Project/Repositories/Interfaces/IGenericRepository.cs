using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq.Expressions;
using Models.Pagination;

namespace Repositories.Interfaces
{

    public interface IGenericRepository<T>
        where T : class
    {
        Task<PagedResult<T>> GetAllAsync(int pageNumber = 1, int pageSize = 10, T? filter = null);
        Task<T?> GetByIdAsync(int id);
        Task<T?> InsertAsync(T entity);
        Task<T?> UpdateAsync(T entity);
        Task<T?> DeleteByIdAsync(int id);
        Task<IEnumerable<T>> BulkInsertAsync(List<T> data);
        Task<IEnumerable<T>> BulkUpdateAsync(List<T> data);
        Task<IEnumerable<T>> BulkUpsertAsync(List<T> data);
        Task<IEnumerable<T>> BulkMergeAsync(List<T> entities, Expression<Func<T, bool>>? deleteFilter = null);
    }
}
