using Models;
using Repositories.Interfaces;
using Services.Interfaces;
using Models.Pagination;

namespace Services.Classes
{
    public class AuthorityService : IAuthorityService
    {
        private readonly IAuthorityRepository _authorityRepository;

        public AuthorityService(IAuthorityRepository authorityRepository)
        {
            _authorityRepository = authorityRepository;
        }

        public async Task<Authority?> InsertAsync(Authority data)
        {
            return await _authorityRepository.InsertAsync(data);
        }

        public async Task<Authority?> UpdateAsync(Authority data)
        {
            return await _authorityRepository.UpdateAsync(data);
        }
        public async Task<PagedResult<Authority>> GetAllAsync(int pageNumber = 1, int pageSize = 10, Authority? filter = null)
        {
            return await _authorityRepository.GetAllAsync(pageNumber, pageSize, filter);
        }

        public async Task<Authority?> GetByIdAsync(int id)
        {
            return await _authorityRepository.GetByIdAsync(id);
        }

        public async Task<Authority?> DeleteByIdAsync(int id)
        {
            return await _authorityRepository.DeleteByIdAsync(id);
        }
        public async Task<IEnumerable<Authority>> BulkInsertAsync(List<Authority> data)
        {
            return await _authorityRepository.BulkInsertAsync(data);
        }
        public async Task<IEnumerable<Authority>> BulkUpdateAsync(List<Authority> data)
        {
            return await _authorityRepository.BulkUpdateAsync(data);
        }
        public async Task<IEnumerable<Authority>> BulkUpsertAsync(List<Authority> data)
        {
            return await _authorityRepository.BulkUpsertAsync(data);
        }
        public async Task<IEnumerable<Authority>> BulkMergeAsync(List<Authority> data)
        {
            return await _authorityRepository.BulkMergeAsync(data);
        }
    }
}