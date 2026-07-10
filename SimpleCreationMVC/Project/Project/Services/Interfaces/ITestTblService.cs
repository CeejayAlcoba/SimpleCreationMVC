using Models;
using Models.Pagination;

namespace Services.Interfaces
{
    public interface ITestTblService
    {
        Task<TestTbl?> InsertAsync(TestTbl data);
        Task<TestTbl?> UpdateAsync(TestTbl data);
        Task<PagedResult<TestTbl>> GetAllAsync(int pageNumber = 1, int pageSize = 10, TestTbl? filter = null);
        Task<TestTbl?> GetByIdAsync(int id);
        Task<TestTbl?> DeleteByIdAsync(int id);
        Task<IEnumerable<TestTbl>> BulkInsertAsync(List<TestTbl> data);
        Task<IEnumerable<TestTbl>> BulkUpdateAsync(List<TestTbl> data);
        Task<IEnumerable<TestTbl>> BulkUpsertAsync(List<TestTbl> data);
        Task<IEnumerable<TestTbl>> BulkMergeAsync(List<TestTbl> data);
    }
}
