using Models;
using Models.Pagination;

namespace Services.Interfaces
{
    public interface IDemeritRecordService
    {
        Task<DemeritRecord?> InsertAsync(DemeritRecord data);
        Task<DemeritRecord?> UpdateAsync(DemeritRecord data);
        Task<PagedResult<DemeritRecord>> GetAllAsync(int pageNumber = 1, int pageSize = 10, DemeritRecord? filter = null);
        Task<DemeritRecord?> GetByIdAsync(int id);
        Task<DemeritRecord?> DeleteByIdAsync(int id);
        Task<IEnumerable<DemeritRecord>> BulkInsertAsync(List<DemeritRecord> data);
        Task<IEnumerable<DemeritRecord>> BulkUpdateAsync(List<DemeritRecord> data);
        Task<IEnumerable<DemeritRecord>> BulkUpsertAsync(List<DemeritRecord> data);
        Task<IEnumerable<DemeritRecord>> BulkMergeAsync(List<DemeritRecord> data);
    }
}
