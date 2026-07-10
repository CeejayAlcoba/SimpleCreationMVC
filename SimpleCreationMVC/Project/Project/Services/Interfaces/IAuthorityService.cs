using Models;
using Models.Pagination;

namespace Services.Interfaces
{
    public interface IAuthorityService
    {
        Task<Authority?> InsertAsync(Authority data);
        Task<Authority?> UpdateAsync(Authority data);
        Task<PagedResult<Authority>> GetAllAsync(int pageNumber = 1, int pageSize = 10, Authority? filter = null);
        Task<Authority?> GetByIdAsync(int id);
        Task<Authority?> DeleteByIdAsync(int id);
        Task<IEnumerable<Authority>> BulkInsertAsync(List<Authority> data);
        Task<IEnumerable<Authority>> BulkUpdateAsync(List<Authority> data);
        Task<IEnumerable<Authority>> BulkUpsertAsync(List<Authority> data);
        Task<IEnumerable<Authority>> BulkMergeAsync(List<Authority> data);
    }
}
