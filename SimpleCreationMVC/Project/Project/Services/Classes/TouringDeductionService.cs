using Models;
using Repositories.Interfaces;
using Services.Interfaces;
using Models.Pagination;

namespace Services.Classes
{
    public class TouringDeductionService : ITouringDeductionService
    {
        private readonly ITouringDeductionRepository _touringDeductionRepository;

        public TouringDeductionService(ITouringDeductionRepository touringDeductionRepository)
        {
            _touringDeductionRepository = touringDeductionRepository;
        }

        public async Task<TouringDeduction?> InsertAsync(TouringDeduction data)
        {
            return await _touringDeductionRepository.InsertAsync(data);
        }

        public async Task<TouringDeduction?> UpdateAsync(TouringDeduction data)
        {
            return await _touringDeductionRepository.UpdateAsync(data);
        }
        public async Task<PagedResult<TouringDeduction>> GetAllAsync(int pageNumber = 1, int pageSize = 10, TouringDeduction? filter = null)
        {
            return await _touringDeductionRepository.GetAllAsync(pageNumber, pageSize, filter);
        }

        public async Task<TouringDeduction?> GetByIdAsync(int id)
        {
            return await _touringDeductionRepository.GetByIdAsync(id);
        }

        public async Task<TouringDeduction?> DeleteByIdAsync(int id)
        {
            return await _touringDeductionRepository.DeleteByIdAsync(id);
        }
        public async Task<IEnumerable<TouringDeduction>> BulkInsertAsync(List<TouringDeduction> data)
        {
            return await _touringDeductionRepository.BulkInsertAsync(data);
        }
        public async Task<IEnumerable<TouringDeduction>> BulkUpdateAsync(List<TouringDeduction> data)
        {
            return await _touringDeductionRepository.BulkUpdateAsync(data);
        }
        public async Task<IEnumerable<TouringDeduction>> BulkUpsertAsync(List<TouringDeduction> data)
        {
            return await _touringDeductionRepository.BulkUpsertAsync(data);
        }
        public async Task<IEnumerable<TouringDeduction>> BulkMergeAsync(List<TouringDeduction> data)
        {
            return await _touringDeductionRepository.BulkMergeAsync(data);
        }
    }
}