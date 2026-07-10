using Models;
using Repositories.Interfaces;
using Services.Interfaces;
using Models.Pagination;

namespace Services.Classes
{
    public class TestTblService : ITestTblService
    {
        private readonly ITestTblRepository _testTblRepository;

        public TestTblService(ITestTblRepository testTblRepository)
        {
            _testTblRepository = testTblRepository;
        }

        public async Task<TestTbl?> InsertAsync(TestTbl data)
        {
            return await _testTblRepository.InsertAsync(data);
        }

        public async Task<TestTbl?> UpdateAsync(TestTbl data)
        {
            return await _testTblRepository.UpdateAsync(data);
        }
        public async Task<PagedResult<TestTbl>> GetAllAsync(int pageNumber = 1, int pageSize = 10, TestTbl? filter = null)
        {
            return await _testTblRepository.GetAllAsync(pageNumber, pageSize, filter);
        }

        public async Task<TestTbl?> GetByIdAsync(int id)
        {
            return await _testTblRepository.GetByIdAsync(id);
        }

        public async Task<TestTbl?> DeleteByIdAsync(int id)
        {
            return await _testTblRepository.DeleteByIdAsync(id);
        }
        public async Task<IEnumerable<TestTbl>> BulkInsertAsync(List<TestTbl> data)
        {
            return await _testTblRepository.BulkInsertAsync(data);
        }
        public async Task<IEnumerable<TestTbl>> BulkUpdateAsync(List<TestTbl> data)
        {
            return await _testTblRepository.BulkUpdateAsync(data);
        }
        public async Task<IEnumerable<TestTbl>> BulkUpsertAsync(List<TestTbl> data)
        {
            return await _testTblRepository.BulkUpsertAsync(data);
        }
        public async Task<IEnumerable<TestTbl>> BulkMergeAsync(List<TestTbl> data)
        {
            return await _testTblRepository.BulkMergeAsync(data);
        }
    }
}