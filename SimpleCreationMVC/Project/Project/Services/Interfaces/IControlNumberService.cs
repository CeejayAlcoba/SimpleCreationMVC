using Models;
using Models.Pagination;

namespace Services.Interfaces
{
    public interface IControlNumberService
    {
        Task<ControlNumber?> InsertAsync(ControlNumber data);
        Task<ControlNumber?> UpdateAsync(ControlNumber data);
        Task<PagedResult<ControlNumber>> GetAllAsync(int pageNumber = 1, int pageSize = 10, ControlNumber? filter = null);
        Task<ControlNumber?> GetByIdAsync(int id);
        Task<ControlNumber?> DeleteByIdAsync(int id);
        Task<IEnumerable<ControlNumber>> BulkInsertAsync(List<ControlNumber> data);
        Task<IEnumerable<ControlNumber>> BulkUpdateAsync(List<ControlNumber> data);
        Task<IEnumerable<ControlNumber>> BulkUpsertAsync(List<ControlNumber> data);
        Task<IEnumerable<ControlNumber>> BulkMergeAsync(List<ControlNumber> data);
    }
}
