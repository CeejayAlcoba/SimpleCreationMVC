using Models;
using Repositories.Interfaces;
using Services.Interfaces;
using Models.Pagination;

namespace Services.Classes
{
    public class DemeritRecordService : IDemeritRecordService
    {
        private readonly IDemeritRecordRepository _demeritRecordRepository;

        public DemeritRecordService(IDemeritRecordRepository demeritRecordRepository)
        {
            _demeritRecordRepository = demeritRecordRepository;
        }

        public async Task<DemeritRecord?> InsertAsync(DemeritRecord data)
        {
            return await _demeritRecordRepository.InsertAsync(data);
        }

        public async Task<DemeritRecord?> UpdateAsync(DemeritRecord data)
        {
            return await _demeritRecordRepository.UpdateAsync(data);
        }
        public async Task<PagedResult<DemeritRecord>> GetAllAsync(int pageNumber = 1, int pageSize = 10, DemeritRecord? filter = null)
        {
            return await _demeritRecordRepository.GetAllAsync(pageNumber, pageSize, filter);
        }

        public async Task<DemeritRecord?> GetByIdAsync(int id)
        {
            return await _demeritRecordRepository.GetByIdAsync(id);
        }

        public async Task<DemeritRecord?> DeleteByIdAsync(int id)
        {
            return await _demeritRecordRepository.DeleteByIdAsync(id);
        }
        public async Task<IEnumerable<DemeritRecord>> BulkInsertAsync(List<DemeritRecord> data)
        {
            return await _demeritRecordRepository.BulkInsertAsync(data);
        }
        public async Task<IEnumerable<DemeritRecord>> BulkUpdateAsync(List<DemeritRecord> data)
        {
            return await _demeritRecordRepository.BulkUpdateAsync(data);
        }
        public async Task<IEnumerable<DemeritRecord>> BulkUpsertAsync(List<DemeritRecord> data)
        {
            return await _demeritRecordRepository.BulkUpsertAsync(data);
        }
        public async Task<IEnumerable<DemeritRecord>> BulkMergeAsync(List<DemeritRecord> data)
        {
            return await _demeritRecordRepository.BulkMergeAsync(data);
        }
    }
}