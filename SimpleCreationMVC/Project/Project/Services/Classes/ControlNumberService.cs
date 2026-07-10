using Models;
using Repositories.Interfaces;
using Services.Interfaces;
using Models.Pagination;

namespace Services.Classes
{
    public class ControlNumberService : IControlNumberService
    {
        private readonly IControlNumberRepository _controlNumberRepository;

        public ControlNumberService(IControlNumberRepository controlNumberRepository)
        {
            _controlNumberRepository = controlNumberRepository;
        }

        public async Task<ControlNumber?> InsertAsync(ControlNumber data)
        {
            return await _controlNumberRepository.InsertAsync(data);
        }

        public async Task<ControlNumber?> UpdateAsync(ControlNumber data)
        {
            return await _controlNumberRepository.UpdateAsync(data);
        }
        public async Task<PagedResult<ControlNumber>> GetAllAsync(int pageNumber = 1, int pageSize = 10, ControlNumber? filter = null)
        {
            return await _controlNumberRepository.GetAllAsync(pageNumber, pageSize, filter);
        }

        public async Task<ControlNumber?> GetByIdAsync(int id)
        {
            return await _controlNumberRepository.GetByIdAsync(id);
        }

        public async Task<ControlNumber?> DeleteByIdAsync(int id)
        {
            return await _controlNumberRepository.DeleteByIdAsync(id);
        }
        public async Task<IEnumerable<ControlNumber>> BulkInsertAsync(List<ControlNumber> data)
        {
            return await _controlNumberRepository.BulkInsertAsync(data);
        }
        public async Task<IEnumerable<ControlNumber>> BulkUpdateAsync(List<ControlNumber> data)
        {
            return await _controlNumberRepository.BulkUpdateAsync(data);
        }
        public async Task<IEnumerable<ControlNumber>> BulkUpsertAsync(List<ControlNumber> data)
        {
            return await _controlNumberRepository.BulkUpsertAsync(data);
        }
        public async Task<IEnumerable<ControlNumber>> BulkMergeAsync(List<ControlNumber> data)
        {
            return await _controlNumberRepository.BulkMergeAsync(data);
        }
    }
}