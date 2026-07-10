using Models;
using Models.Pagination;

namespace Services.Interfaces
{
    public interface ITouringDeductionService
    {
        Task<TouringDeduction?> InsertAsync(TouringDeduction data);
        Task<TouringDeduction?> UpdateAsync(TouringDeduction data);
        Task<PagedResult<TouringDeduction>> GetAllAsync(int pageNumber = 1, int pageSize = 10, TouringDeduction? filter = null);
        Task<TouringDeduction?> GetByIdAsync(int id);
        Task<TouringDeduction?> DeleteByIdAsync(int id);
        Task<IEnumerable<TouringDeduction>> BulkInsertAsync(List<TouringDeduction> data);
        Task<IEnumerable<TouringDeduction>> BulkUpdateAsync(List<TouringDeduction> data);
        Task<IEnumerable<TouringDeduction>> BulkUpsertAsync(List<TouringDeduction> data);
        Task<IEnumerable<TouringDeduction>> BulkMergeAsync(List<TouringDeduction> data);
    }
}
